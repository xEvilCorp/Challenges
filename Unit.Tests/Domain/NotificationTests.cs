using Common.Tests;
using FluentAssertions;
using System.Linq;
using Vaquinha.Domain;
using Vaquinha.Domain.Entities;
using Xunit;

namespace Unit.Tests.Domain
{
    [Collection(nameof(PersonFixtureCollection))]
    public class NotificationTests : IClassFixture<CreditCardFixture>
    {
        private readonly CreditCardFixture fix;
        private readonly IDomainNotificationService ns;

        public NotificationTests(CreditCardFixture fixture)
        {
            ns = new DomainNotificationService();
            fix = fixture;
        }


        [Fact]
        [Trait("NotificationService", "NotificationService_AddingEntity")]
        public void NotificationService_CheckForAddEntity()
        {
            CartaoCredito card = fix.Empty();
            card.Valido();

            ns.Adicionar(card);
            var notifications = ns.RecuperarErrosDominio().Select(a => a.MensagemErro);

            notifications.Should().HaveCount(4, because: "4 required fields are empty.");
            notifications.Should().Contain("O campo CVV deve ser preenchido", because: "the card cvv field is required but it's empty.");
            notifications.Should().Contain("O campo Validade deve ser preenchido", because: "the card expiration date field is required but it's missing.");
            notifications.Should().Contain("O campo Nome Titular deve ser preenchido", because: "the name field is required but it's empty.");
            notifications.Should().Contain("O campo Número de cartão de crédito deve ser preenchido", because: "the card number field is required but it's empty.");

            ns.PossuiErros.Should().BeTrue(because: "An empty invalid credit card entity has been added.");
        }
        [Fact]
        [Trait("NotificationService", "NotificationService_NewClass")]
        public void NotificationService_CheckForNewClassCreation()
        {
            var notification = new DomainNotificationService();
            notification.PossuiErros.Should().BeFalse(because: "no notification has been added yet");
        }

        [Fact]
        [Trait("NotificationService", "NotificationService_AddNotification")]
        public void NotificationService_CheckForAddNotification()
        {
            ns.Adicionar(new DomainNotification("RequiredField", "O campo Nome é obrigatório"));
            ns.PossuiErros.Should().BeTrue(because: "a notification for required field has been added.");

            var notifications = ns.RecuperarErrosDominio().Select(a => a.MensagemErro);
            notifications.Should().Contain("O campo Nome é obrigatório", because: "a notification for required field has been added containing this message");
        }
    }
}
