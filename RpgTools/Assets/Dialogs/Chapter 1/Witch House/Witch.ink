EXTERNAL SetPortrait(name)
EXTERNAL SetVar(name, value)
EXTERNAL GetVar(name)
EXTERNAL AddItems(name, count)
#using SetPortrait
#using SetVar
#using GetVar
#using AddItems
~SetPortrait("Witch")
{GetVar("witchEncounter") == -1:
    ->FirstEncouner
  - else:
    {GetVar("witchHelp") == -1:
    ->Question
  - else:
    ->Notes
}

}

=== FirstEncouner ===
~SetVar("witchEncounter", 1)
Witaj, tyhyhyhyhy
*[Czy to prawda że jesteś wiedźmą?]
Tyhyhyhy, ciekawe pytanie
niektórzy mnie tak nazywają ale ja wolę myśleć o sobie jako o naukowczyni
tak jak wy studiujecie potwory i ich nawyki tak ja studiuje nieco starszą numeryczną wiedze
->FirstQuestion

=== FirstQuestion ===
Jak już przy tym jesteśmy to nie pomógł byś może naukowcowi w potrzebie?
*[Z chęcią pomogę]
->Help
*[Przepraszam ale jestem zajęty]
->HelpDecline

=== Help ===
~SetVar("witchHelp", 1)
Wspaniale!
Tyhyhyhyhy
Jakiś czas temu przez naszą wioskę przeszła straszna wichura i porwała wszystkie moje notatki
Taka skarbnica analizy matematycznej zniknęła w mgnieniu oka
Co za strata
Jeśli byłbyś na tyle miły i pomógł mi je pozbierać to z pewnością sowicie cię wynagrodze
Tyhyhyhyhy
Wiesz co?
Masz tutaj coś na drogę za to że jesteś taki miły
~AddItems("Potion", 1)
~SetPortrait("")
Otrzymałeś 10 mikstur! Możesz ich użyć aby się podleczyć!
~SetPortrait("Witch")
No co tak na mnie patrzy?
Przyrządzanie mikstur to takie moje małe hobby niemające niczego wspólnego z byciem wiedźmą
-> END

=== HelpDecline===
Co za szkoda
Tyhyhyhy
->END

=== Question ===
Co cię tutaj znowu sprowadza?
Zdecydowałeś się jednak pomóc?
*[Tak]
->Help
*[Nie]
->HelpDecline


=== Notes ===
Jak idzie tobie zbieranie moich notatek?
Z tego co widzę zostało ich jeszcze troche
->END

