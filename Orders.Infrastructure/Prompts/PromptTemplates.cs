namespace Orders.Infrastructure.Prompts;

public static class PromptTemplates
{
	public const string OrderExtraction = """
        Jesteś parserem HTML. Otrzymasz dokument HTML z informacjami o zamówieniu.
        Wyodrębnij numer zamówienia, nazwę każdego produktu, ilość i cenę.
        Zwróć wynik wyłącznie w postaci poprawnego obiektu JSON o strukturze:
        {
            "id": liczba,
            "products": [
                {
                    "productName": "nazwa produktu",
                    "quantity": liczba,
                    "price": liczba
                }
            ]
        }

        Nie dodawaj żadnych komentarzy, tekstu poza JSON ani formatowania Markdown.
        Zwróć tylko ten obiekt JSON.
        """;
}