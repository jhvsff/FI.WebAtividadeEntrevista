using System.ComponentModel.DataAnnotations;
using System.Linq;

public class CpfAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        if (value == null)
            return true;

        var cpf = value.ToString().Replace(".", "").Replace("-", "");

        if (cpf.Length != 11 || !cpf.All(char.IsDigit))
            return true;

        return IsCpfValid(cpf);
    }

    private bool IsCpfValid(string cpf)
    {
        if (cpf.Distinct().Count() == 1)
            return false;

        var sum = 0;
        for (int i = 0; i < 9; i++)
            sum += (cpf[i] - '0') * (10 - i);

        var remainder = sum % 11;
        var firstDigit = remainder < 2 ? 0 : 11 - remainder;

        if (cpf[9] - '0' != firstDigit)
            return false;

        sum = 0;
        for (int i = 0; i < 10; i++)
            sum += (cpf[i] - '0') * (11 - i);

        remainder = sum % 11;
        var secondDigit = remainder < 2 ? 0 : 11 - remainder;

        return cpf[10] - '0' == secondDigit;
    }
}