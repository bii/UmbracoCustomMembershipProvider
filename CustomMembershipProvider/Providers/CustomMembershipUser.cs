using System;
using System.Web.Security;

namespace CustomMembershipProvider.Providers
{
    public class CustomMembershipUser : MembershipUser
    {
        public int ZipCode { get; set; }

        public CustomMembershipUser(
            string providerName,
            string name,
            object providerUserKey,
            string email,
            string passwordQuestion,
            string comment,
            bool isApproved,
            bool isLockedOut,
            DateTime creationDate,
            DateTime lastLoginDate,
            DateTime lastActivityDate,
            DateTime lastPasswordChangedDate,
            DateTime lastLockoutDate,
            int zipCode) :
            base(
                providerName,
                name,
                providerUserKey,
                email,
                passwordQuestion,
                comment,
                isApproved,
                isLockedOut,
                creationDate,
                lastLoginDate,
                lastActivityDate,
                lastPasswordChangedDate,
                lastLockoutDate)
        {
            ZipCode = zipCode;
        }
    }
}