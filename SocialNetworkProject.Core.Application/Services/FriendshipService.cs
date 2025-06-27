using AutoMapper;
using Microsoft.AspNetCore.Http;
using SocialNetworkProject.Core.Application.Dtos.Account;
using SocialNetworkProject.Core.Application.Dtos.FriendRequest;
using SocialNetworkProject.Core.Application.Helpers;
using SocialNetworkProject.Core.Application.Interfaces;
using SocialNetworkProject.Core.Application.ViewModels.Friend;
using SocialNetworkProject.Core.Application.ViewModels.Friendship;
using SocialNetworkProject.Core.Domain.Common.Enums;
using SocialNetworkProject.Core.Domain.Entities;
using SocialNetworkProject.Core.Domain.Interfaces.Generic;

namespace SocialNetworkProject.Core.Application.Services
{
    public class FriendshipService : IFriendshipService
    {
        private readonly IGenericRepository<FriendRequest> _friendRequestRepo;
        private readonly IGenericRepository<Friendship> _friendshipRepo;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        private readonly string _currentUserId;

        public FriendshipService(IGenericRepository<FriendRequest> friendRequestRepo,
                                 IGenericRepository<Friendship> friendshipRepo,
                                 IAccountService accountService,
                                 IMapper mapper,
                                 IHttpContextAccessor httpContextAccessor)
        {
            _friendRequestRepo = friendRequestRepo;
            _friendshipRepo = friendshipRepo;
            _accountService = accountService;
            _mapper = mapper;
            _currentUserId = httpContextAccessor.HttpContext?.Session.Get<AuthenticationResponse>("user")?.Id!;
        }

        public async Task<FriendRequestViewModel> GetFriendRequestsAsync(string userId)
        {
            var allRequests = await _friendRequestRepo.GetAllAsync();
            var allUsers = await _accountService.GetAllUsersAsync();
            var currentUserFriends = await GetFriendIds(userId);

            var received = allRequests
                .Where(r => r.ReceiverId == userId && r.Status == RequestStatus.Pending)
                .ToList();

            var sent = allRequests
                .Where(r => r.RequesterId == userId)
                .ToList();

            var receivedViewModels = new List<FriendRequestInfoViewModel>();
            foreach (var r in received)
            {
                receivedViewModels.Add(new FriendRequestInfoViewModel
                {
                    RequestId = r.Id,
                    UserId = r.RequesterId,
                    UserName = allUsers.FirstOrDefault(u => u.Id == r.RequesterId)?.UserName,
                    RequestDate = r.RequestedAt,
                    MutualFriends = await GetMutualFriendsCount(userId, r.RequesterId, currentUserFriends)
                });
            }

            var sentViewModels = new List<FriendRequestInfoViewModel>();
            foreach (var r in sent)
            {
                sentViewModels.Add(new FriendRequestInfoViewModel
                {
                    RequestId = r.Id,
                    UserId = r.ReceiverId,
                    UserName = allUsers.FirstOrDefault(u => u.Id == r.ReceiverId)?.UserName,
                    RequestDate = r.RequestedAt,
                    Status = r.Status.ToString(),
                    MutualFriends = await GetMutualFriendsCount(userId, r.ReceiverId, currentUserFriends)
                });
            }

            return new FriendRequestViewModel
            {
                ReceivedRequests = receivedViewModels,
                SentRequests = sentViewModels
            };
        }

        public async Task<AddFriendViewModel> GetPotentialFriendsAsync(string currentUserId, string? searchTerm)
        {
            var allUsers = await _accountService.GetAllUsersAsync();
            var currentUserFriends = await GetFriendIds(currentUserId);
            var pendingRequests = await _friendRequestRepo.GetAllAsync();

            var pendingRequestPartners = pendingRequests
                .Where(r => (r.RequesterId == currentUserId || r.ReceiverId == currentUserId) && r.Status == RequestStatus.Pending)
                .Select(r => r.RequesterId == currentUserId ? r.ReceiverId : r.RequesterId)
                .ToHashSet();

            var potentialFriends = allUsers
                .Where(u => u.Id != currentUserId &&
                            u.IsActive &&
                            !currentUserFriends.Contains(u.Id) &&
                            !pendingRequestPartners.Contains(u.Id))
                .ToList();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                potentialFriends = potentialFriends.Where(u => u.UserName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            var potentialFriendViewModels = new List<PotentialFriendViewModel>();
            foreach (var user in potentialFriends)
            {
                potentialFriendViewModels.Add(new PotentialFriendViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    ProfilePictureUrl = user.ProfilePictureUrl,
                    MutualFriendsCount = await GetMutualFriendsCount(currentUserId, user.Id, currentUserFriends)
                });
            }

            return new AddFriendViewModel
            {
                SearchTerm = searchTerm,
                PotentialFriends = potentialFriendViewModels
            };
        }

        public async Task SendFriendRequestAsync(string requesterId, string receiverId)
        {
            var request = new CreateFriendRequestDto
            {
                RequesterId = requesterId,
                ReceiverId = receiverId
            };
            var entity = _mapper.Map<FriendRequest>(request);
            await _friendRequestRepo.AddAsync(entity);
        }

        public async Task RespondToFriendRequestAsync(int requestId, string currentUserId, RequestStatus newStatus)
        {
            var request = await _friendRequestRepo.GetByIdAsync(requestId);
            if (request == null || request.ReceiverId != currentUserId)
            {
                throw new Exception("Solicitud no válida o no autorizado.");
            }

            request.Status = newStatus;
            request.RespondedAt = DateTime.UtcNow;
            await _friendRequestRepo.UpdateAsync(requestId, request);

            if (newStatus == RequestStatus.Accepted)
            {
                await _friendshipRepo.AddAsync(new Friendship { Id = default, UserId = request.RequesterId, FriendId = request.ReceiverId, CreatedAt = DateTime.UtcNow });
                await _friendshipRepo.AddAsync(new Friendship { Id = default, UserId = request.ReceiverId, FriendId = request.RequesterId, CreatedAt = DateTime.UtcNow });
            }
        }

        public async Task DeleteFriendRequestAsync(int requestId, string currentUserId)
        {
            var request = await _friendRequestRepo.GetByIdAsync(requestId);
            if (request == null || request.RequesterId != currentUserId)
            {
                throw new Exception("Solicitud no válida o no autorizado.");
            }
            await _friendRequestRepo.DeleteAsync(requestId);
        }

        public async Task<FriendRequestInfoViewModel?> GetFriendRequestInfoAsync(int requestId, string currentUserId)
        {
            var request = await _friendRequestRepo.GetByIdAsync(requestId);
            if (request == null) return null;

            var otherUserId = request.RequesterId == currentUserId ? request.ReceiverId : request.RequesterId;
            var allUsers = await _accountService.GetAllUsersAsync();
            var otherUser = allUsers.FirstOrDefault(u => u.Id == otherUserId);

            if (otherUser == null) return null;

            return new FriendRequestInfoViewModel
            {
                RequestId = requestId,
                UserId = otherUser.Id,
                UserName = otherUser.UserName
            };
        }

        private async Task<HashSet<string>> GetFriendIds(string userId)
        {
            var friendships = await _friendshipRepo.GetAllAsync();
            return friendships.Where(f => f.UserId == userId).Select(f => f.FriendId).ToHashSet();
        }

        private async Task<int> GetMutualFriendsCount(string user1Id, string user2Id, HashSet<string> user1Friends)
        {
            if (user1Id == user2Id) return 0;
            var user2Friends = await GetFriendIds(user2Id);
            return user1Friends.Intersect(user2Friends).Count();
        }

        public async Task<List<FriendViewModel>> GetAllFriendsAsync(string userId)
        {
            var allUsers = await _accountService.GetAllUsersAsync();
            var friendshipIds = await GetFriendIds(userId);

            return allUsers
                .Where(u => friendshipIds.Contains(u.Id))
                .Select(u => new FriendViewModel
                {
                    UserId = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    UserName = u.UserName,
                    ProfilePictureUrl = u.ProfilePictureUrl ?? "/images/default-profile.png"
                }).ToList();
        }

        public async Task DeleteFriendshipAsync(string userId, string friendId)
        {
            var friendships = await _friendshipRepo.GetAllAsync();

            var friendship1 = friendships.FirstOrDefault(f => f.UserId == userId && f.FriendId == friendId);
            if (friendship1 != null)
            {
                await _friendshipRepo.DeleteAsync(friendship1.Id);
            }

            var friendship2 = friendships.FirstOrDefault(f => f.UserId == friendId && f.FriendId == userId);
            if (friendship2 != null)
            {
                await _friendshipRepo.DeleteAsync(friendship2.Id);
            }
        }
        public async Task<int> GetPendingRequestCountAsync(string userId)
        {
            var allRequests = await _friendRequestRepo.GetAllAsync();
            return allRequests.Count(r => r.ReceiverId == userId && r.Status == RequestStatus.Pending);
        }
    }
}
