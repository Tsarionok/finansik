using Finansik.Domain.Authentication;
using Finansik.Domain.Authorization;
using Finansik.Domain.Authorization.Category;
using Finansik.Domain.Exceptions;
using FluentAssertions;
using JetBrains.Annotations;
using Moq;
using Moq.Language.Flow;

namespace Finansik.Domain.Tests.Authorization;

[TestSubject(typeof(IntentionManager))]
public class IntentionManagerShould
{
    private readonly IntentionManager _sut;
    private readonly ISetup<IIntentionResolver<CategoryIntention>,bool> _isAllowedSetup;

    public IntentionManagerShould()
    {
        var mockIdentityProvider = new Mock<IIdentityProvider>();
        var mockResolver = new Mock<IIntentionResolver<CategoryIntention>>();
        _isAllowedSetup = mockResolver.Setup(r => r.IsAllowed(It.IsAny<IIdentity>(), It.IsAny<CategoryIntention>()));
        mockIdentityProvider.Setup(ip => ip.Current).Returns(new Mock<IIdentity>().Object);
        
        var resolvers = new List<IIntentionResolver> { mockResolver.Object };
        _sut = new IntentionManager(resolvers, mockIdentityProvider.Object);
    }

    [Fact]
    public void IsAllowed_WithMatchingResolver_ReturnsTrue()
    {
        _isAllowedSetup.Returns(true);
        
        var result = _sut.IsAllowed(CategoryIntention.Create);
        
        result.Should().BeTrue();
    }

    [Fact]
    public void IsAllowed_WithNoMatchingResolver_ReturnsFalse()
    {
        var result = _sut.IsAllowed(CategoryIntention.Create);

        result.Should().BeFalse();
    }

    [Fact]
    public void ThrowIfForbidden_WhenIntentionIsNotAllowed_ThrowsIntentionManagerException()
    {
        _isAllowedSetup.Returns(false);

        var actual = _sut.Invoking(sut => sut.ThrowIfForbidden(CategoryIntention.Create));
        
        actual.Should().Throw<IntentionManagerException>();
    }

    [Fact]
    public void ThrowIfForbidden_WhenIntentionIsAllowed_DoesNotThrow()
    {
        _isAllowedSetup.Returns(true);
        
        var actual = _sut.Invoking(sut => sut.ThrowIfForbidden(CategoryIntention.Create));
        
        actual.Should().NotThrow();
    }
}