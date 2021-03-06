﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Reward.Domain.SeedWork;

namespace Reward.Domain.AggregateModel
{
    public interface IUserRepository : IRepository<User>
    {
        User Add(User user);
        Task<User> GetAsync(Guid userIdentityGuid);
        Task<User> GetAsync(string phoneNumber);
        Task<User> GetCountryAdminAsync(int countryId);
        Task<IList<RewardRule>> GetRewardRules();
    }
}