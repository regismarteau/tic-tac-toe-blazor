using AngleSharp.Dom;
using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components;

namespace AcceptanceTests.Extensions;

public static class RenderedComponentExtensions
{
    extension<T>(IRenderedComponent<T> component) where T : IComponent
    {
        public IElement FindByDataTest(DataTestSelector dataTest) => component.WaitForElement(dataTest);

        public void WaitForNoElement(DataTestSelector dataTest) => component.WaitForAssertion(() => component.FindAll(dataTest).Should().BeEmpty());
    }
}
