using System;
using System.Text.RegularExpressions;

namespace SimpleValueObjects.Examples
{
    public class UserName : WrapperEquitableObject<UserName, string>
    {
        public UserName(string userName)
            : base(userName)
        {
            if (userName == null)
            {
                throw new ArgumentException(
                    "UserName cannot be null.");
            }

            if (!_userNamePattern.IsMatch(userName))
            {
                throw new ArgumentException(
                    $"UserName should match pattern {_userNamePattern}, but found '{userName}' instead.");
            }
        }

        private readonly Regex _userNamePattern = new Regex("[a-z0-9-]{5,25}");
    }
}