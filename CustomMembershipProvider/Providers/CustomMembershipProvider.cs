using System;
using System.Collections.Generic;
using System.Web.Security;

namespace CustomMembershipProvider.Providers
{
    public class CustomMembershipProvider : MembershipProvider
    {
        private readonly UserDataProvider _userDataProvider = new UserDataProvider();

        public override MembershipUser CreateUser(string username, string password, string email,
            string passwordQuestion,
            string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            throw new System.NotImplementedException();
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password,
            string newPasswordQuestion,
            string newPasswordAnswer)
        {
            throw new System.NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new System.NotImplementedException();
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new System.NotImplementedException();
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new System.NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new System.NotImplementedException();
        }

        public override bool ValidateUser(string username, string password)
        {
            var member = _userDataProvider.Find(username);
            var databasePassword = member.Password;
            var storedHashedPass = PasswordHelper.StoredPassword(databasePassword, out var salt);
            var hashed = PasswordHelper.EncryptOrHashPassword(password, salt, PasswordFormat);

            return storedHashedPass == hashed;
        }

        public override bool UnlockUser(string userName)
        {
            throw new System.NotImplementedException();
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            var member = _userDataProvider.FindByProviderUserKey( ((Guid)providerUserKey).ToString("D"));
            return MemberPropertiesToCustomMembershipUser(member.LoginName);
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            var member = _userDataProvider.Find(username);
            return MemberPropertiesToCustomMembershipUser(member.LoginName);
        }

        public override string GetUserNameByEmail(string email)
        {
            var member = _userDataProvider.FindByEmail(email);

            return member.LoginName;
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new System.NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            var userCollection = new MembershipUserCollection();

            var cmsMembers = _userDataProvider.FindAll();

            foreach (var member in cmsMembers)
            {
                userCollection.Add(
                    MemberPropertiesToCustomMembershipUser(member.LoginName));
            }

            totalRecords = userCollection.Count;

            return userCollection;
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new System.NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize,
            out int totalRecords)
        {
            throw new System.NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize,
            out int totalRecords)
        {
            throw new System.NotImplementedException();
        }

        public override bool EnablePasswordRetrieval { get; }
        public override bool EnablePasswordReset { get; }
        public override bool RequiresQuestionAndAnswer { get; }
        public override string ApplicationName { get; set; }
        public override int MaxInvalidPasswordAttempts { get; }
        public override int PasswordAttemptWindow { get; }
        public override bool RequiresUniqueEmail { get; }
        public override MembershipPasswordFormat PasswordFormat => MembershipPasswordFormat.Hashed;
        public override int MinRequiredPasswordLength { get; }
        public override int MinRequiredNonAlphanumericCharacters { get; }
        public override string PasswordStrengthRegularExpression { get; }
        
        private CustomMembershipUser MemberPropertiesToCustomMembershipUser(string loginName)
        {
            Dictionary<string, string> memberProperties = _userDataProvider.GetMemberProperties(loginName, out var name, out var providerUserKey);
            CmsMember member = _userDataProvider.Find(loginName);
            DateTime lastPasswordChangedDate = DateTime.MinValue, lastLockoutDate = DateTime.MinValue;
            string comment ="";
            bool isApproved = false, isLockedOut = false;
            DateTime lastLoginDate = DateTime.MinValue;
            int zipCode = 0;

            foreach (var memberProperty in memberProperties)
            {
                switch (memberProperty.Key)
                {
                    case "umbracoMemberComments":
                        comment = memberProperty.Value;
                        break;
                    case "umbracoMemberLockedOut":
                        isLockedOut = int.Parse(memberProperty.Value) == 1;
                        break;
                    case "umbracoMemberApproved":
                        isApproved = int.Parse(memberProperty.Value) == 1;
                        break;
                    case "umbracoMemberLastLogin":
                        lastLoginDate = DateTime.Parse(memberProperty.Value);
                        break;
                    case "umbracoMemberLastLockoutDate":
                        lastLockoutDate = DateTime.Parse(memberProperty.Value);
                        break;
                    case "umbracoMemberLastPasswordChangeDate":
                        lastPasswordChangedDate = DateTime.Parse(memberProperty.Value);
                        break;
                    case "zipCode":
                        zipCode = int.Parse(memberProperty.Value);
                        break;
                }
            }

            return new CustomMembershipUser(
                "UmbracoMembershipProvider", 
                member.LoginName,
                providerUserKey,
                member.Email,
                "",
                comment,
                isApproved,
                isLockedOut,
                DateTime.Now,
                lastLoginDate,
                DateTime.Now,
                lastPasswordChangedDate,
                lastLockoutDate, 
                zipCode);
        }
    }
}