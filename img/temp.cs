

var claims = new List<Claim>
{
    new (claimTypes.Name, "vasia"),
    new (claimTypes.Email, "vasia@mail.com"),
    new (ClaimTypes.Role, "User")
};

var identity = new ClaimsIdentity(claims, "MyScheme");





var principal = new ClaimsPrincipal(identity);