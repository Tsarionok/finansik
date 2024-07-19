using Finansik.Domain.Authentication;
using Finansik.Domain.Authorization.Category;
using FluentAssertions;
using JetBrains.Annotations;
using Moq;
using Moq.Language.Flow;

namespace Finansik.Domain.Tests.Authorization.Group;

[TestSubject(typeof(CategoryIntentionResolver))]
public class CategoryIntentionResolverShould
{
    private readonly CategoryIntentionResolver _sut;
    private readonly Mock<IIdentity> _mockIdentity;
    private readonly ISetup<IIdentity,bool> _isAuthenticatedSetup;

    public CategoryIntentionResolverShould()
    {
        _sut = new CategoryIntentionResolver();
        _mockIdentity = new Mock<IIdentity>();
        _isAuthenticatedSetup = _mockIdentity.Setup(identity => identity.IsAuthenticated());
    }

    [Theory]
    [InlineData(CategoryIntention.Create)]
    [InlineData(CategoryIntention.Rename)]
    [InlineData(CategoryIntention.Delete)]
    public void IsAllowed_AuthenticatedUser_ReturnsTrue(CategoryIntention intention)
    {
        _isAuthenticatedSetup.Returns(true);
        
        var actual = _sut.IsAllowed(_mockIdentity.Object, intention);

        actual.Should().BeTrue();
    }

    [Theory]
    [InlineData(CategoryIntention.Create)]
    [InlineData(CategoryIntention.Rename)]
    [InlineData(CategoryIntention.Delete)]
    public void IsAllowed_NotAuthenticatedUser_ReturnsFalse(CategoryIntention intention)
    {
        _isAuthenticatedSetup.Returns(false);
        
        var actual = _sut.IsAllowed(_mockIdentity.Object, intention);
        
        actual.Should().BeFalse();
    }

    [Fact]
    public void IsAllowed_UnknownIntention_ReturnsFalse()
    {
        _isAuthenticatedSetup.Returns(true);
        
        var actual = _sut.IsAllowed(_mockIdentity.Object, (CategoryIntention)999);

        actual.Should().BeFalse();
    }
}