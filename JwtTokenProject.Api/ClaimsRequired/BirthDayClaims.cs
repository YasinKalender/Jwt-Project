using Microsoft.AspNetCore.Authorization;

namespace JwtTokenProject.Api.ClaimsRequired
{
    public class BirthDayClaims : IAuthorizationRequirement
    {
        public int Age { get; set; }

        public BirthDayClaims(int age)
        {
            Age = age;
        }
    }

    public class BirthDayClaimsHandler : AuthorizationHandler<BirthDayClaims>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, BirthDayClaims requirement)
        {
            var birthDate = context.User.FindFirst("BirthDate");

            if (birthDate == null)
            {
                context.Fail();
                return Task.CompletedTask;
            }

            var today = DateTime.Now;

            var age = today.Year - Convert.ToDateTime(birthDate.Value).Year;

            if (age >= requirement.Age)
            {
                context.Succeed(requirement);
            }

            else
            {
                context.Fail();
                return Task.CompletedTask;
            }

            return Task.CompletedTask;
        }


    }
}
