using LanguageExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLMChatBot.Application.Abstractions;
using TwitchLMChatBot.Models;

namespace TwitchLMChatBot.Application.Services
{
    public class AccessControlService : IAccessControlService
    {
        private readonly IAccessControlRepository _accessControlRepository;

        public AccessControlService(IAccessControlRepository accessControlRepository)
        {
            _accessControlRepository = accessControlRepository;
        }

        public AccessControl Get()
        {
            return _accessControlRepository.FindAll().FirstOrDefault()!;
        }


        public void Set(SetAccessControlRequest request)
        {
            var item = _accessControlRepository.FindAll().FirstOrDefault() ?? new AccessControl();

            item.Subscribers = request.Subscribers;
            item.Followers = request.Followers;
            item.Moderators = request.Moderators;
            item.Unrestricted = request.Unrestricted;
            item.Vips = request.Vips;

            if (item.Id != 0)
            {
                _accessControlRepository.Update(item);
            }
            else
            {
                _accessControlRepository.Insert(item);
            }

        }

        public bool Check(CheckAccessRequest checkRequest)
        {
            var accessControl = Get();

            if (accessControl.Unrestricted)
                return true;

            var conditions = new[]
            {
                (accessControl.Followers, checkRequest.IsFollower),
                (accessControl.Subscribers, checkRequest.IsSubscriber),
                (accessControl.Moderators, checkRequest.IsModerator),
                (accessControl.Vips, checkRequest.IsVip)
            };

            return conditions.Any(condition => condition.Item1 && condition.Item2);

        }
    }

    public class SetAccessControlRequest
    {
        public bool Unrestricted { get; set; }
        public bool Followers { get; set; }
        public bool Subscribers { get; set; }
        public bool Moderators { get; set; }
        public bool Vips { get; set; }
    }
}
