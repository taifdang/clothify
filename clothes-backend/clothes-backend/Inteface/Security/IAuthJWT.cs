namespace clothes_backend.Inteface.JWT
{
    public interface IAuthJWT
    {
        void verify();
        void hash();
        void generate_token();
    }
}
