using AuthByJWT.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthByJWT.Repository.EntityConfigurations
{
    public class AuthRefreshTokenConfiguration : IEntityTypeConfiguration<AuthRefreshToken>
    {
        public void Configure(EntityTypeBuilder<AuthRefreshToken> builder)
        {
            builder.HasKey(x => x.UserId);
        }
    }
}
