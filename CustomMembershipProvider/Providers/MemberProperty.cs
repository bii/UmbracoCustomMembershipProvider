using System;

namespace CustomMembershipProvider.Providers
{
    public class MemberProperty
    {
        public string Alias { get; set; }
        public string Val { get; set; }
        public string Name { get; set; }
        public Guid ProviderUserKey { get; set; }
    }
}