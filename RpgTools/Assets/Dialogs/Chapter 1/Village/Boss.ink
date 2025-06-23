EXTERNAL SetPortrait(name)
EXTERNAL SetVar(name, value)
EXTERNAL GetVar(name)
#using SetPortrait
#using SetVar
#using GetVar

~SetPortrait("Boss")
{GetVar("bossTalk") == -1:
    ->First
  - else:
    Przepraszam Fex ale jestem zajęty
    ->END
}


===First===
~SetVar("bossTalk", 1)
Oczom nie wierze
Kto tu w końcu raczył wstać
Mam nadzieję że skoro aż tak się wyspałeś to dasz dzisiaj z siebie 200%
Pamiętaj dzisiaj jest dzień w którym jeśli wraz z innymi udowodnicie swojej wartości dołączycie do grona łowców i przewodników
Swoją drogą Alice ciebie szukała. Mówiła że będzie czekać na tej polanie co zwykle 
Zdawała się całkiem wkurzona 
Jeśli zdecydujesz się pójść jej poszukać to powiedz strażnikowi przy bramie że ja ciebie wysłałem bo inaczej cie nie wypuści
A teraz jeśli pozwolisz też mam jeszcze dużo obowiązków na głowie
Inicjacja sama się nie przygotuje
~SetVar("canLeaveVillage", 1)
->END