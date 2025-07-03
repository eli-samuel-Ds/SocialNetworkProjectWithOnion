using AutoMapper;
using Microsoft.AspNetCore.Http;
using SocialNetworkProject.Core.Application.Dtos.Account;
using SocialNetworkProject.Core.Application.Helpers;
using SocialNetworkProject.Core.Application.Interfaces;
using SocialNetworkProject.Core.Application.ViewModels.Battle;
using SocialNetworkProject.Core.Domain.Common.Enums;
using SocialNetworkProject.Core.Domain.Entities;
using SocialNetworkProject.Core.Domain.Interfaces.Generic;

namespace SocialNetworkProject.Core.Application.Services
{
    public class BattleService : IBattleService
    {
        private readonly IGenericRepository<Battle> _battleRepo;
        private readonly IGenericRepository<Ship> _shipRepo;
        private readonly IGenericRepository<ShipPosition> _shipPositionRepo;
        private readonly IGenericRepository<Attack> _attackRepo;
        private readonly IFriendshipService _friendshipService;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        private readonly string _currentUserId;
        private const int BoardSize = 12;

        public BattleService(
            IGenericRepository<Battle> battleRepo,
            IGenericRepository<Ship> shipRepo,
            IGenericRepository<ShipPosition> shipPositionRepo,
            IGenericRepository<Attack> attackRepo,
            IFriendshipService friendshipService,
            IAccountService accountService,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _battleRepo = battleRepo;
            _shipRepo = shipRepo;
            _shipPositionRepo = shipPositionRepo;
            _attackRepo = attackRepo;
            _friendshipService = friendshipService;
            _accountService = accountService;
            _mapper = mapper;
            _currentUserId = httpContextAccessor.HttpContext?.Session.Get<AuthenticationResponse>("user")?.Id!;
        }

        public async Task<AttackResultViewModel> ProcessAttackAsync(int battleId, string attackerId, int x, int y)
        {
            var battle = await _battleRepo.GetByIdAsync(battleId);
            var result = new AttackResultViewModel { IsGameOver = false };

            if (battle == null)
            {
                result.ErrorMessage = "La partida no fue encontrada o es inválida.";
                return result;
            }

            if (battle.Status != BattleStatus.InProgress)
            {
                result.ErrorMessage = "La partida no está en curso.";
                return result;
            }
            if (battle.CurrentTurnPlayerId != attackerId)
            {
                result.ErrorMessage = "No es tu turno de atacar.";
                return result;
            }

            var allAttacks = await _attackRepo.GetAllAsync();
            if (allAttacks.Any(a => a.BattleId == battleId && a.AttackerId == attackerId && a.X == x && a.Y == y))
            {
                result.ErrorMessage = "Ya has atacado esta celda.";
                return result;
            }

            var opponentId = battle.Player1Id == attackerId ? battle.Player2Id : battle.Player1Id;
            var opponentShips = (await _shipRepo.GetAllAsync()).Where(s => s.BattleId == battleId && s.OwnerId == opponentId);
            var allPositions = await _shipPositionRepo.GetAllAsync();
            var opponentShipPositions = allPositions.Where(p => opponentShips.Any(s => s.Id == p.ShipId));

            var hitPosition = opponentShipPositions.FirstOrDefault(p => p.X == x && p.Y == y);
            result.IsHit = hitPosition != null;

            await _attackRepo.AddAsync(new Attack
            {
                Id = 0,
                BattleId = battleId,
                AttackerId = attackerId,
                X = x,
                Y = y,
                IsHit = result.IsHit,
                AttackedAt = System.DateTime.UtcNow
            });

            if (result.IsHit)
            {
                var allPlayerAttacks = (await _attackRepo.GetAllAsync()).Where(a => a.BattleId == battleId && a.AttackerId == attackerId && a.IsHit);
                var opponentShipsWithPositions = (await _shipRepo.GetAllAsync()).Where(s => s.BattleId == battleId && s.OwnerId == opponentId)
                    .Select(s => new
                    {
                        Ship = s,
                        Positions = allPositions.Where(p => p.ShipId == s.Id).ToList()
                    });

                var allOpponentShipsSunk = opponentShipsWithPositions.All(shipInfo =>
                    shipInfo.Positions.All(pos => allPlayerAttacks.Any(atk => atk.X == pos.X && atk.Y == pos.Y))
                );

                if (allOpponentShipsSunk)
                {
                    result.IsGameOver = true;
                    result.WinnerId = attackerId;
                    battle.Status = BattleStatus.Completed;
                    battle.WinnerId = attackerId;
                    battle.EndedAt = System.DateTime.UtcNow;
                }
            }

            if (!result.IsGameOver)
            {
                battle.CurrentTurnPlayerId = opponentId;
            }

            await _battleRepo.UpdateAsync(battle.Id, battle);
            return result;
        }

        public async Task<BattleshipIndexViewModel> GetBattleshipIndexAsync(string userId)
        {
            var allBattles = await _battleRepo.GetAllAsync();
            var allUsers = await _accountService.GetAllUsersAsync();
            var userBattles = allBattles.Where(b => b.Player1Id == userId || b.Player2Id == userId).ToList();

            var vm = new BattleshipIndexViewModel();

            vm.ActiveGames = userBattles
               .Where(b => b.Status != BattleStatus.Completed)
               .Select(b => new ActiveGameViewModel
               {
                   BattleId = b.Id,
                   OpponentUserName = allUsers.FirstOrDefault(u => u.Id == (b.Player1Id == userId ? b.Player2Id : b.Player1Id))?.UserName ?? "Desconocido",
                   StartedAt = b.StartedAt
               }).ToList();

            var completedGames = userBattles.Where(b => b.Status == BattleStatus.Completed).ToList();
            vm.GameHistory = completedGames
               .OrderByDescending(b => b.EndedAt)
               .Select(b => new GameHistorySummaryViewModel
               {
                   BattleId = b.Id,
                   OpponentUserName = allUsers.FirstOrDefault(u => u.Id == (b.Player1Id == userId ? b.Player2Id : b.Player1Id))?.UserName ?? "Desconocido",
                   StartedAt = b.StartedAt,
                   EndedAt = b.EndedAt,
                   Result = b.WinnerId == userId ? "Ganada" : "Perdida",
                   WinnerUserName = b.WinnerId == userId ? "Yo" : allUsers.FirstOrDefault(u => u.Id == b.WinnerId)?.UserName ?? "Desconocido"
               }).ToList();

            vm.GameHistoryStats.TotalGames = completedGames.Count;
            vm.GameHistoryStats.GamesWon = completedGames.Count(b => b.WinnerId == userId);
            vm.GameHistoryStats.GamesLost = completedGames.Count(b => b.WinnerId != userId);

            return vm;
        }

        public async Task<StartNewGameViewModel> GetFriendsForNewGameAsync(string userId, string? searchTerm)
        {
            var allFriends = await _friendshipService.GetAllFriendsAsync(userId);
            var allBattles = await _battleRepo.GetAllAsync();

            var friendsInActiveGames = allBattles
               .Where(b => b.Status != BattleStatus.Completed && (b.Player1Id == userId || b.Player2Id == userId))
               .Select(b => b.Player1Id == userId ? b.Player2Id : b.Player1Id)
               .ToHashSet();

            var availableFriends = allFriends
               .Where(f => !friendsInActiveGames.Contains(f.UserId))
               .ToList();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                availableFriends = availableFriends.Where(f => f.UserName.Contains(searchTerm, System.StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return new StartNewGameViewModel
            {
                SearchTerm = searchTerm,
                AvailableFriends = availableFriends.Select(f => new PotentialOpponentViewModel
                {
                    UserId = f.UserId,
                    UserName = f.UserName,
                    ProfilePictureUrl = f.ProfilePictureUrl
                }).ToList()
            };
        }

        public async Task<int> StartNewGameAsync(string player1Id, string player2Id)
        {
            var battle = new Battle
            {
                Id = 0,
                Player1Id = player1Id,
                Player2Id = player2Id,
                Status = BattleStatus.Setup,
                StartedAt = System.DateTime.UtcNow,
                CurrentTurnPlayerId = player1Id
            };

            var createdBattle = await _battleRepo.AddAsync(battle);

            var shipTypes = new List<(ShipType type, int size)>
            {
                (ShipType.Carrier, 5),
                (ShipType.Battleship, 4),
                (ShipType.Destroyer, 3),
                (ShipType.Submarine, 3),
                (ShipType.PatrolBoat, 2)
            };

            foreach (var player in new[] { player1Id, player2Id })
            {
                foreach (var shipInfo in shipTypes)
                {
                    await _shipRepo.AddAsync(new Ship
                    {
                        Id = 0,
                        BattleId = createdBattle.Id,
                        OwnerId = player,
                        Type = shipInfo.type,
                        Size = shipInfo.size,
                        IsPositioned = false
                    });
                }
            }
            return createdBattle.Id;
        }

        public async Task<ShipPlacementViewModel> GetShipPlacementViewModelAsync(int battleId, string userId)
        {
            var battle = await _battleRepo.GetByIdAsync(battleId);
            if (battle == null || (battle.Player1Id != userId && battle.Player2Id != userId))
            {
                return null;
            }

            var vm = new ShipPlacementViewModel { BattleId = battleId, UserId = userId };
            var allShips = await _shipRepo.GetAllAsync();
            var userShips = allShips.Where(s => s.BattleId == battleId && s.OwnerId == userId).ToList();

            vm.UnplacedShips = userShips
                .Where(s => !s.IsPositioned)
                .Select(s => new ShipViewModel { ShipId = s.Id, Type = s.Type, Size = s.Size })
                .ToList();

            vm.Board = await GetUserPlacementBoardAsync(battleId, userId);

            if (vm.UnplacedShips.Count == 0)
            {
                var opponentId = battle.Player1Id == userId ? battle.Player2Id : battle.Player1Id;
                var opponentReady = await AreAllShipsPlaced(battleId, opponentId);
                if (!opponentReady)
                {
                    vm.IsWaitingForOpponent = true;
                }
            }

            return vm;
        }

        public async Task<PlaceShipResultViewModel> PlaceShipAsync(PlaceShipViewModel vm)
        {
            var result = new PlaceShipResultViewModel { Success = false };

            if (!vm.SelectedShipId.HasValue || !vm.StartX.HasValue || !vm.StartY.HasValue || !vm.Direction.HasValue)
            {
                result.ErrorMessage = "Información incompleta. Debe seleccionar un barco, una celda y una dirección.";
                return result;
            }

            var ship = await _shipRepo.GetByIdAsync(vm.SelectedShipId.Value);
            if (ship == null || ship.OwnerId != vm.UserId || ship.BattleId != vm.BattleId)
            {
                result.ErrorMessage = "Barco no válido seleccionado.";
                return result;
            }

            var coordinates = new System.Collections.Generic.List<(int X, int Y)>();
            for (int i = 0; i < ship.Size; i++)
            {
                int x = vm.StartX.Value;
                int y = vm.StartY.Value;

                switch (vm.Direction.Value)
                {
                    case ShipDirection.Arriba: y -= i; break;
                    case ShipDirection.Abajo: y += i; break;
                    case ShipDirection.Izquierda: x -= i; break;
                    case ShipDirection.Derecha: x += i; break;
                }
                coordinates.Add((x, y));
            }

            var allBoardPositions = await _shipPositionRepo.GetAllAsync();
            var userShips = (await _shipRepo.GetAllAsync()).Where(s => s.BattleId == vm.BattleId && s.OwnerId == vm.UserId);
            var existingPositions = allBoardPositions
                .Where(p => userShips.Any(s => s.Id == p.ShipId))
                .Select(p => (p.X, p.Y)).ToHashSet();

            foreach (var (x, y) in coordinates)
            {
                if (x < 0 || x >= BoardSize || y < 0 || y >= BoardSize)
                {
                    result.ErrorMessage = "El barco quedaría fuera del tablero. Por favor, elija otra celda o dirección.";
                    return result;
                }
                if (existingPositions.Contains((x, y)))
                {
                    result.ErrorMessage = "El barco se superpondría con otro ya posicionado. Por favor, elija otra celda o dirección.";
                    return result;
                }
            }

            foreach (var (x, y) in coordinates)
            {
                await _shipPositionRepo.AddAsync(new ShipPosition
                {
                    Id = 0,
                    ShipId = ship.Id,
                    X = x,
                    Y = y
                });
            }

            ship.IsPositioned = true;
            await _shipRepo.UpdateAsync(ship.Id, ship);
            await CheckAndStartGameAsync(vm.BattleId);

            result.Success = true;
            return result;
        }

        public async Task<GameViewModel> GetGameViewModelAsync(int battleId, string userId)
        {
            var battle = await _battleRepo.GetByIdAsync(battleId);
            if (battle == null || (battle.Player1Id != userId && battle.Player2Id != userId)) return null;

            var opponentId = battle.Player1Id == userId ? battle.Player2Id : battle.Player1Id;
            var opponent = await _accountService.GetUserByIdAsync(opponentId);

            var vm = new GameViewModel
            {
                BattleId = battleId,
                CurrentUserId = userId,
                OpponentUserName = opponent.UserName,
                IsUserTurn = battle.CurrentTurnPlayerId == userId,
                AttackBoard = await GetAttackBoardAsync(battleId, userId, opponentId)
            };

            return vm;
        }

        public async Task SurrenderGameAsync(int battleId, string surrenderingPlayerId)
        {
            var battle = await _battleRepo.GetByIdAsync(battleId);
            if (battle == null || battle.Status == BattleStatus.Completed) return;

            battle.Status = BattleStatus.Completed;
            battle.EndedAt = System.DateTime.UtcNow;
            battle.WinnerId = battle.Player1Id == surrenderingPlayerId ? battle.Player2Id : battle.Player1Id;

            await _battleRepo.UpdateAsync(battle.Id, battle);
        }

        public async Task<GameHistoryDetailViewModel> GetGameHistoryDetailsAsync(int battleId, string userId)
        {
            var battle = await _battleRepo.GetByIdAsync(battleId);
            if (battle == null) return null;

            var opponentId = battle.Player1Id == userId ? battle.Player2Id : battle.Player1Id;

            return new GameHistoryDetailViewModel
            {
                BattleId = battleId,
                UserAttackBoard = await GetAttackBoardAsync(battleId, userId, opponentId, true),
                OpponentAttackBoard = await GetAttackBoardAsync(battleId, opponentId, userId, true),
                UserPlacementBoard = await GetUserPlacementBoardAsync(battleId, userId)
            };
        }

        public async Task<UserBoardViewModel> GetUserBoardAsync(int battleId, string userId)
        {
            return new UserBoardViewModel
            {
                BattleId = battleId,
                PlacementBoard = await GetUserPlacementBoardAsync(battleId, userId)
            };
        }

        public async Task<bool> AreAllShipsPlaced(int battleId, string userId)
        {
            var allShips = await _shipRepo.GetAllAsync();
            return allShips.Where(s => s.BattleId == battleId && s.OwnerId == userId).All(s => s.IsPositioned);
        }

        public async Task CheckAndStartGameAsync(int battleId)
        {
            var battle = await _battleRepo.GetByIdAsync(battleId);
            if (battle.Status != BattleStatus.Setup) return;

            var allPlayer1Placed = await AreAllShipsPlaced(battleId, battle.Player1Id);
            var allPlayer2Placed = await AreAllShipsPlaced(battleId, battle.Player2Id);

            if (allPlayer1Placed && allPlayer2Placed)
            {
                battle.Status = BattleStatus.InProgress;
                await _battleRepo.UpdateAsync(battleId, battle);
            }
        }

        private async Task<CellViewModel[,]> GetUserPlacementBoardAsync(int battleId, string userId)
        {
            var board = new CellViewModel[BoardSize, BoardSize];
            var allShips = await _shipRepo.GetAllAsync();
            var userShips = allShips.Where(s => s.BattleId == battleId && s.OwnerId == userId && s.IsPositioned);
            var allPositions = await _shipPositionRepo.GetAllAsync();
            var userShipPositions = allPositions.Where(p => userShips.Any(s => s.Id == p.ShipId));

            for (int i = 0; i < BoardSize; i++)
            {
                for (int j = 0; j < BoardSize; j++)
                {
                    board[i, j] = new CellViewModel { X = i, Y = j, Status = CellStatus.Empty };
                }
            }

            foreach (var pos in userShipPositions)
            {
                board[pos.X, pos.Y].Status = CellStatus.Ship;
            }

            return board;
        }

        private async Task<CellViewModel[,]> GetAttackBoardAsync(int battleId, string attackerId, string opponentId, bool isGameOver = false)
        {
            var board = new CellViewModel[BoardSize, BoardSize];
            var allAttacks = await _attackRepo.GetAllAsync();
            var playerAttacks = allAttacks.Where(a => a.BattleId == battleId && a.AttackerId == attackerId);

            for (int i = 0; i < BoardSize; i++)
            {
                for (int j = 0; j < BoardSize; j++)
                {
                    board[i, j] = new CellViewModel { X = i, Y = j, Status = CellStatus.Empty };
                }
            }

            foreach (var attack in playerAttacks)
            {
                board[attack.X, attack.Y].Status = attack.IsHit ? CellStatus.Hit : CellStatus.Miss;
            }

            return board;
        }

        public async Task CheckForAbandonedGames()
        {
            var allBattles = await _battleRepo.GetAllAsync();
            var allAttacks = await _attackRepo.GetAllAsync();

            var gamesInProgress = allBattles.Where(b => b.Status == BattleStatus.InProgress);

            foreach (var battle in gamesInProgress)
            {
                var lastAttack = allAttacks
                    .Where(a => a.BattleId == battle.Id)
                    .OrderByDescending(a => a.AttackedAt)
                    .FirstOrDefault();

                System.DateTime lastMoveTime = lastAttack?.AttackedAt ?? battle.StartedAt;

                if ((System.DateTime.UtcNow - lastMoveTime).TotalHours > 48)
                {
                    battle.Status = BattleStatus.Completed;
                    battle.EndedAt = System.DateTime.UtcNow;
                    battle.WinnerId = battle.CurrentTurnPlayerId == battle.Player1Id ? battle.Player2Id : battle.Player1Id;
                    await _battleRepo.UpdateAsync(battle.Id, battle);
                }
            }
        }
    }
}
