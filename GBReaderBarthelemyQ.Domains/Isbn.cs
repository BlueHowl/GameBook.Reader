using GBReaderBarthelemyQ.Domains.Exceptions;
using System.Text.RegularExpressions;

namespace GBReaderBarthelemyQ.Domains;

/// <summary>
/// Classe Isbn
/// </summary>
public class Isbn
{
    private string? _isbnNumber;

    public string IsbnNumber
    {
        get => _isbnNumber!;
        set => _isbnNumber = value;
    }


    /// <summary>
    /// Constructeur de la classe isbn sur base d'un numéro existant
    /// </summary>
    /// <param name="isbnNumber">(String) numéro isbn à stocker</param>
    /// <exception cref="IsbnNotValidException">si le numéro isbn est invalide</exception>
    public Isbn(string isbnNumber)
    {
        if (Regex.Match(isbnNumber, "^\\d-\\d{6}-\\d{2}-(\\d|x|0)$", RegexOptions.IgnoreCase).Success)
        {
            string isbnStart = isbnNumber[..^1];
            string isbnVerif = isbnNumber[^2..];

            if (GetIsbnVerification(isbnStart).CompareTo(isbnVerif) == 0)
            {
                IsbnNumber = isbnNumber;
            }
            else
            {
                throw new IsbnNotValidException("Le code isbn est invalide, le code de vérification ne correspond pas");
            }
        }
        else
        {
            throw new IsbnNotValidException("Le code isbn est invalide, veuillez respecter la syntaxe");
        }
    }

    /**
     * Retourne le résultat du calcul de l'isbn (9 premiers chiffres de l’ISBN chacuns multiplié par un poids allant de 10 à 2)
     * @param isbnStart, premiere partie du numéro isbn (9 premiers chiffres)
     * @return (int) résultat du calcul de vérification
     */
    public string GetIsbnVerification(string isbnStart)
    {
        int sum = GetIsbnSum(isbnStart.Replace("-", "").ToCharArray());
        int code = 11 - sum % 11;

        return (code >= 11 ? "-0" : (code >= 10 ? "-x" : string.Format("-{0}", code)));
    }

    private int GetIsbnSum(char[] sequence)
    {
        int sum = 0;
        int weight = 10;

        foreach (char c in sequence)
        {
            sum += (int)char.GetNumericValue(c) * weight--;
        }

        return sum;
    }

    public override string ToString() => IsbnNumber;

    public override bool Equals(object? obj) => obj is Isbn isbn && IsbnNumber == isbn.IsbnNumber;

    public override int GetHashCode() => HashCode.Combine(IsbnNumber);
}

