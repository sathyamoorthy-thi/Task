using System.ComponentModel.DataAnnotations;


namespace PaymentClaimApi
{
    

public class User
{

    [Key]
    
    public int UserId {get;set;}
    public string UserName {get;set;}
    public string Password {get;set;}
    
}

public class Jwt
{

    public string key {get;set;}

    public string Issuer {get;set;}
    public string Audience {get;set;}
    public string Subject {get;set;}


}
}