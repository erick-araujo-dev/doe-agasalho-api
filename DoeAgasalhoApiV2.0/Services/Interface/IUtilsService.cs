namespace DoeAgasalhoApiV2._0.Services.Interface
{
    public interface IUtilsService
    {
        int GetPontoColetaIdFromToken();

        int GetUserIdFromToken();

        void ValidateActive(string active);

        void ValidateStringField(string value, string field, int maxLength, bool isNumeric);
    }
}
