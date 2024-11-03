using AutoMapper;
using FluentEmail.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Serilog;
using ShoppingPlatform.BLL.Dto.AccountDto;
using ShoppingPlatform.BLL.Dto.JwtDto;
using ShoppingPlatform.BLL.Response;
using ShoppingPlatform.BLL.Service.Abstraction;
using ShoppingPlatform.DAL.Entity;

namespace ShoppingPlatform.BLL.Service.Implementation;

public class AccountService : IAccountService
{
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;
    // private readonly IFluentEmail _fluentEmail;
    // private readonly IVerificationService _verificationService;

    public AccountService(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, ITokenService tokenService, IMapper mapper)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _tokenService = tokenService;
        _mapper = mapper;

    }
    
    public async Task<ApiResponse<JwtTokenResponse>> Login(LoginDto loginDto, IConfiguration configuration)
    {
        Log.Logger.Information(nameof(Login) + "logging in for {Email}", loginDto.Email);
        var response = new ApiResponse<JwtTokenResponse>()
        {
            Data = new JwtTokenResponse()
        };

        try
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            
            if (user == null)
            {
                response.Failure("User not found");
                Log.Logger.Warning("user not found with this {Email}", loginDto.Email);
                return response;
            }
            
            var roles = await _userManager.GetRolesAsync(user);
    
            var signInResult =
                await _signInManager.PasswordSignInAsync(user.UserName ?? throw new InvalidOperationException(), loginDto.Password, loginDto.IsPersist, false);

            Log.Logger.Information("SignInResult: {Result}", signInResult.ToString());
            
            if (signInResult.Succeeded)
            {
                var generatedToken =
                    await _tokenService.GenerateToken(new TokenRequest { Email = loginDto.Email }, configuration, roles);

                response.Data.Token = generatedToken.Token;
                response.Data.ExpireDate = generatedToken.ExpireDate;
                
                response.Success(response.Data, 200);
            }
            else
            {
                response.Failure("Username or password is not correct");
                Log.Logger.Warning("Username or password is not correct for {Email}", loginDto.Email);
                return response;
            }
        }
        catch (Exception e)
        {
            response.Failure(e.Message, 500);
            Log.Logger.Error(nameof(Login) + "Login failed for {Email}", loginDto.Email);
            throw;
        }

        return response;
    }

    public async Task<ApiResponse<RegisterDto>> Register(RegisterDto registerDto)
    {
        Log.Logger.Information(nameof(Register) + "Registering for {Email}", registerDto.Email);
        var response = new ApiResponse<RegisterDto>();

        try
        {
            var mappedUser = _mapper.Map<AppUser>(registerDto);
            var userEntity = await _userManager.CreateAsync(mappedUser, registerDto.Password);

            if (userEntity.Succeeded)
            {
                response.Success(registerDto, 201);
                Log.Logger.Information(nameof(Register) + "User created successfully");
            }

            // if (userEntity.Succeeded)
            // {
            //     var verificationCode = new Random().Next(100000, 999999).ToString();
            //
            //     await _verificationService.StoreVerificationCode(registerDto.Email, verificationCode);
            //     
            //     var emailResponse = await _fluentEmail
            //         .To(registerDto.Email)
            //         .Subject("Your Verification Code")
            //         .Body($"Your verification code is: {verificationCode}")
            //         .SendAsync();
            //     
            //     if (emailResponse.Successful)
            //     {
            //         response.Success(registerDto, 201);
            //         Log.Logger.Information("Verification code sent to {Email}", registerDto.Email);
            //     }
            //     else
            //     {
            //         response.Failure("Failed to send verification email.", 500);
            //         Log.Logger.Warning("Failed to send verification code to {Email}", registerDto.Email);
            //         return response;
            //     }
            // }
            // else
            // {
            //     response.Failure("Failed to create user.", 500);
            //     Log.Logger.Warning("Failed to create user with {Email}", registerDto.Email);
            //     return response;
            // }
        }
        catch (Exception e)
        {
            response.Failure(e.Message, 500);
            Log.Logger.Error(nameof(Register) + "Error happened while registering user: {Message}", e.Message);
            throw;
        }

        return response;
    }

    // public async Task<ApiResponse<bool>> Verify(VerifyDto verifyDto)
    // {
    //     Log.Logger.Information(nameof(Verify) + " Verifying for {Email}", verifyDto.Email);
    //     var response = new ApiResponse<bool>();
    //
    //     try
    //     {
    //         // Validate the verification code
    //         if (await _verificationService.IsCodeValid(verifyDto.Email, verifyDto.Code))
    //         {
    //             // Retrieve the user
    //             var user = await _userManager.FindByEmailAsync(verifyDto.Email);
    //             if (user != null)
    //             {
    //                 // Confirm the email (or activate the account)
    //                 user.EmailConfirmed = true;
    //                 var result = await _userManager.UpdateAsync(user);
    //
    //                 if (result.Succeeded)
    //                 {
    //                     response.Success(true, 200);
    //                     Log.Logger.Information("User account verified successfully for {Email}", verifyDto.Email);
    //                 }
    //                 else
    //                 {
    //                     response.Failure("Failed to update user account.", 500);
    //                     Log.Logger.Warning("Failed to update user account for {Email}", verifyDto.Email);
    //                     return response;
    //                 }
    //             }
    //             else
    //             {
    //                 response.Failure("User not found.", 404);
    //                 Log.Logger.Warning("User not found for {Email}", verifyDto.Email);
    //                 return response;
    //             }
    //         }
    //         else
    //         {
    //             response.Failure("Invalid or expired verification code.", 400);
    //             Log.Logger.Warning("Invalid verification code provided for {Email}", verifyDto.Email);
    //             return response;
    //         }
    //     }
    //     catch (Exception e)
    //     {
    //         response.Failure(e.Message, 500);
    //         Log.Logger.Error(nameof(Verify) + " Error while verifying code for {Email}: {Message}", verifyDto.Email, e.Message);
    //         throw;
    //     }
    //
    //     return response;
    // }
    //
    // public async Task<ApiResponse<bool>> ResendVerificationCode(string email)
    // {
    //     Log.Logger.Information(nameof(ResendVerificationCode) + " Resending verification code for {Email}", email);
    //     var response = new ApiResponse<bool>();
    //
    //     try
    //     {
    //         // Check if we can resend the verification code
    //         if (await _verificationService.CanResendCode(email))
    //         {
    //             var newCode = new Random().Next(100000, 999999).ToString();
    //
    //             // Store the new code
    //             await _verificationService.StoreVerificationCode(email, newCode);
    //
    //             // Send the new code
    //             var emailResponse = await _fluentEmail
    //                 .To(email)
    //                 .Subject("Your New Verification Code")
    //                 .Body($"Your new verification code is: {newCode}")
    //                 .SendAsync();
    //
    //             if (emailResponse.Successful)
    //             {
    //                 response.Success(true, 200);
    //                 Log.Logger.Information("Verification code resent successfully to {Email}", email);
    //             }
    //             else
    //             {
    //                 response.Failure("Failed to resend verification email.", 500);
    //                 Log.Logger.Warning("Failed to resend verification code to {Email}", email);
    //                 return response;
    //             }
    //         }
    //         else
    //         {
    //             response.Failure("Cannot resend code yet. Please wait a while before trying again.", 400);
    //             Log.Logger.Warning("Attempt to resend verification code too soon for {Email}", email);
    //             return response;
    //         }
    //     }
    //     catch (Exception e)
    //     {
    //         response.Failure(e.Message, 500);
    //         Log.Logger.Error(nameof(ResendVerificationCode) + " Error while resending verification code for {Email}: {Message}", email, e.Message);
    //         throw;
    //     }
    //
    //     return response;
    // }

    public async Task<ApiResponse<bool>> Logout()
    {
        Log.Logger.Information(nameof(Logout) + "Logging out");
        var response = new ApiResponse<bool>();

        try
        {
            await _signInManager.SignOutAsync();
            response.Success(true, 200);
            Log.Logger.Information(nameof(Logout) + "Logout completed successfully");
        }
        catch (Exception e)
        {
            response.Failure(e.Message, 500);
            Log.Logger.Error(e.Message + " Error happened while logging out");
            throw;
        }

        return response;
    }
}