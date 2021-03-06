﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Transaction.Domain.AggregateModel;

namespace Transaction.Infrastructure.EntityConfigurations
{
    public class UserEntityTypeConfigurations: IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {

            builder.HasKey(b => b.Id);

            builder.Ignore(b => b.DomainEvents);
            var navigation = builder.Metadata.FindNavigation(nameof(User.Transactions));

            // DDD Patterns comment:
            //Set as field (New since EF 1.1) to access the Transactions collection property through its field
            navigation.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
