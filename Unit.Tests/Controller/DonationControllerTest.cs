using AutoMapper;
using Common.Tests;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NToastNotify;
using System;
using System.Collections.Generic;
using System.Text;
using Vaquinha.Domain;
using Vaquinha.Domain.Entities;
using Vaquinha.Domain.ViewModels;
using Vaquinha.MVC.Controllers;
using Vaquinha.Service;
using Xunit;

namespace Unit.Tests.Controller
{
    [Collection(nameof(DonationFixtureCollection))]
    public class DonationControllerTest :  IClassFixture<DonationFixture>, IClassFixture<AddressFixture>, IClassFixture<CreditCardFixture>
    {
        #region variables
        private Mock<IMapper> mapper;
        private readonly AddressFixture addFix;
        private readonly Doacao validDonation;
        private readonly DonationFixture donFix;
        private readonly CreditCardFixture credFix;
        private DoacoesController donationController;
        private Mock<IPaymentService>  polenService;
        private readonly IDoacaoService donationService;
        private Mock<IToastNotification> toastNotification;
        private Mock<ILogger<DoacoesController>> logger;
        private IDomainNotificationService notificationService;
        private readonly DoacaoViewModel  validDonationVM;
        private readonly Mock<GloballAppConfig> appConfig;
        private Mock<IDoacaoRepository> donationRepository;
        #endregion variables

        public DonationControllerTest(DonationFixture donationFix, AddressFixture addressFix, CreditCardFixture creditFix)
        {
            credFix = creditFix;
            donFix = donationFix;
            addFix = addressFix;

            appConfig = new Mock<GloballAppConfig>();
            donationRepository = new Mock<IDoacaoRepository>();
            notificationService = new DomainNotificationService();
            toastNotification = new Mock<IToastNotification>();
            polenService = new Mock<IPaymentService>();
            logger = new Mock<ILogger<DoacoesController>>();
            mapper = new Mock<IMapper>();

            validDonation = donFix.Valid();
            validDonation.AdicionarFormaPagamento(credFix.Valid());
            validDonation.AdicionarEnderecoCobranca(addFix.Valid());

            validDonationVM = donFix.ValidVM();
            validDonationVM.EnderecoCobranca = addressFix.ValidVM();
            validDonationVM.FormaPagamento = credFix.ValidVM();

            mapper.Setup(a => a.Map<DoacaoViewModel, Doacao>(validDonationVM)).Returns(validDonation);
            donationService = new DoacaoService(mapper.Object, donationRepository.Object, notificationService);
        }

        [Fact]
        [Trait("DonationController", "DonationController_InvalidEntry")]
        public void DonationController_CheckForInvalidData()
        {
            var invalidDonation = donFix.Empty();
            var invalidDonationVM = new DoacaoViewModel();
            mapper.Setup(a => a.Map<DoacaoViewModel, Doacao>(invalidDonationVM)).Returns(invalidDonation);
            donationController = new DoacoesController(donationService, notificationService, toastNotification.Object);
            
            var response = donationController.Create(invalidDonationVM);

            response.Should().BeOfType<ViewResult>();
            mapper.Verify(a => a.Map<DoacaoViewModel, Doacao>(invalidDonationVM), Times.Once);
            donationRepository.Verify(a => a.AdicionarAsync(invalidDonation), Times.Never);
            toastNotification.Verify(a => a.AddErrorToastMessage(It.IsAny<string>(), It.IsAny<LibraryOptions>()), Times.Once);
            (response as ViewResult).Model.Should().BeOfType<DoacaoViewModel>();
            ((response as ViewResult).Model as DoacaoViewModel).Should().Be(invalidDonationVM);
        }

        [Fact]
        [Trait("DonationController", "DonationController_AddValidEntry")]
        public void DonationController_CheckForValidEntry()
        {
            donationController = new DoacoesController(donationService, notificationService, toastNotification.Object);

            IActionResult response = donationController.Create(validDonationVM);

            mapper.Verify(a => a.Map<DoacaoViewModel, Doacao>(validDonationVM), Times.Once);
            toastNotification.Verify(a => a.AddSuccessToastMessage(It.IsAny<string>(), It.IsAny<LibraryOptions>()), Times.Once);
            response.Should().BeOfType<RedirectToActionResult>();
            (response as RedirectToActionResult).ActionName.Should().Be("Index");
            (response as RedirectToActionResult).ControllerName.Should().Be("Home");
        }
    }
}
