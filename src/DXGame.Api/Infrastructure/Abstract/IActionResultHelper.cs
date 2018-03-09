using System;
using System.Threading.Tasks;
using DXGame.Common.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace DXGame.Api.Infrastructure.Abstract
{
    public interface IActionResultHelper
    {
        IActionResultHelper Validate(Func<bool> func);
        IActionResultErrorHandler Return<T>(Func<Task<T>> action);
    }
}