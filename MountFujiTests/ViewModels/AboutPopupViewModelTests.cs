﻿using FluentAssertions;
using Mopups.Interfaces;
using Moq;
using MountFuji.ViewModels;

namespace MountFujiTests.ViewModels;

public class AboutPopupViewModelTests
{
    private Mock<IPopupNavigation> popupNavigationMock;
        
        
    [SetUp]
    public void Setup()
    {
        popupNavigationMock = new Mock<IPopupNavigation>();
    }

    [Test]
    public async Task CloseCommand_WhenInvoked_ShouldPopTheWindowOffTheStack()
    {
        var sut = CreateSut();
        await sut.CloseCommand.ExecuteAsync(null);
        popupNavigationMock.Verify(d => d.PopAsync(true), Times.Once);
    }


    private AboutPopupViewModel CreateSut()
    {
        return new AboutPopupViewModel(popupNavigationMock.Object);
    }
    
}