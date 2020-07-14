using System;
using NUnit.Framework;
using CustomMembershipProvider.Providers;

namespace CustomMembershipProviderTest
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void TestFindByLoginName()
        {
            var provider = new UserDataProvider();
            var member = provider.Find("bilal.isa");
            Assert.IsNotNull(member);
            Assert.AreEqual(member.Email, "bilal.isa@pm.me");
        }
        
        [Test]
        public void TestGetMemberPropertiesByLoginName()
        {
            var provider = new UserDataProvider();
            var properties = provider.GetMemberProperties("bilal.isa", out var name, out var providerUserKey);
            Assert.IsNotNull(properties);
            Assert.IsNotEmpty(name);
            Assert.IsNotEmpty(providerUserKey);
        }

        [Test]
        public void TestFindByProviderUserKey()
        {
            var provider = new UserDataProvider();
            var cmsMember = provider.FindByProviderUserKey("F063FFB5-1498-4D70-BA6E-AF2123302222");
            Assert.IsNotNull(cmsMember);
        }
    }
}