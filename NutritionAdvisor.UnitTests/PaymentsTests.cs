using FluentAssertions;
using Moq;
using NutritionAdvisor.Application.Common.Interfaces;
using NutritionAdvisor.Application.Payments.Commands.CreateCheckout;
using Xunit;

namespace NutritionAdvisor.UnitTests;

public class PaymentsTests
{
    [Fact]
    public async Task CreateCheckout_ShouldReturnUrl()
    {
        var mockStripe = new Mock<IPaymentService>();
        mockStripe.Setup(x => x.CreateSubscriptionCheckoutSessionAsync(It.IsAny<Guid>(), "test@test.com"))
            .ReturnsAsync("http://checkout.url");

        var handler = new CreateCheckoutSessionCommandHandler(mockStripe.Object);
        var result = await handler.Handle(new CreateCheckoutSessionCommand(Guid.NewGuid(), "test@test.com"), CancellationToken.None);

        result.Should().Be("http://checkout.url");
    }
}