﻿using Finansik.Domain.Authentication;

namespace Finansik.Domain.UseCases.SignIn;

public interface ISignInUseCase
{
    Task<(IIdentity identity, string token)> Execute(SignInCommand command, CancellationToken cancellationToken);
}