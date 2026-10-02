using AngleSharp.Dom;
using Bunit;
using Microsoft.AspNetCore.Components;

namespace AcceptanceTests.Extensions;

public static class RenderedComponentExtensions
{
    extension<T>(IRenderedComponent<T> component) where T : IComponent
    {
        public IElement FindByDataTest(string dataTest)
        {
            return component.Find($"[data-test=\"{dataTest}\"]");
        }
    }
}
