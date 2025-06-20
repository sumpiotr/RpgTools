EXTERNAL SetPortrait(name)
EXTERNAL ShowBackground(name)
EXTERNAL HideBackground()
EXTERNAL SetVar(name, value)
EXTERNAL GetVar(name)
#using SetPortrait
#using Background
#using SetVar
#using GetVar

{
  - GetVar("Tutorial") == -1:
    ->Start
  - GetVar("Tutorial") == 1:
    ->Skill
  - GetVar("Tutorial") == 2:
    ->SkillUsed
  - GetVar("Tutorial") == 3:
    ->TutorialEnd
  - else:
    ->END
}





===Start===
~SetPortrait("Alice")
Ok, skup się 
~SetPortrait("Fex")
Pamiętasz wszystko?
~SetPortrait("Alice")
Oczywiście ale jak to sprawi że poczujesz się lepiej to możesz zrobić mi szybki recap bo mam wrażenie że mi tutaj zaraz zemdlejesz
~SetPortrait("Fex")
Ej wcale nie jestem nerwowy! 
~SetPortrait("Alice")
Taaa jasne
~SetPortrait("Fex")
W każdym razie widzę że jesteś całkiem zmęczona swoim poprzednim treningiem. W takim stanie nie użyjesz żadnego flashy ruchu. Wykonaj podstawowy atak albo przyjmij obronną podstawę aby zregenerować część swojej energi!
Wykonując atak zadasz podstawowy dmg a broniąc się zapobiegniesz połowie nadchodzących obrażeń. Ponoć atak to najlepsza obrona ale na nic się nie zda jeśli potwór sprzątnie cię jednym atakiem
~SetVar("Tutorial", 1)
->END

===Skill===
~SetPortrait("Fex")
Super! Powinnaś być w stanie użyć teraz jednej ze swoich umiejętności!
Kluczem do sukcesu w każdym polowaniu jest odkrycie słabości potwora z którym walczysz i użycie ich przeciwko niemu
Z reguły typu potwora oraz jego słabości możesz domyślić się po jego wyglądzie. Jeśli jednak nie jesteś pewna, zawsze możesz użyć umiejętności Zbadaj 
Wytłumaczę wtedy co i jak
A i nie kosztuje ona tury więc się nie przejmuj 
Przy pomocy swojego elektrycznego miecza bez problemu powinnaś być w stanie uderzyć w słabość tych Kaplutów
~SetVar("Tutorial", 2)
->END

===SkillUsed===
~SetPortrait("Fex")
Super! Właśnie o to chodziło!
Trafiając w słabość potwora nie tylko robisz mu większą krzywdę ale także masz większą szansę na nałożenie efektu zgodnego z typem ataku jakiego używasz
Hmm?
Mówisz że przydałby się szybki recap efektów i typów?
Dobra to jedziemy:
Ogień: słaby przeciwko wodzie, skuteczny przeciwko lód. Efekt: podpalenie (cel traci 10% swojego hp co turę)

Lód: słaby przeciwko ogień . Efekt: zamrożenie (cel zostaje znacznie spowolniony)

Ziemia: To nasz typ jako że ludzie pochodzą z ziemi. Brak naturalnych słabości i efektu

Elektryczność: słaby przeciwko ziemia, skuteczny przeciwko woda. Efekt: porażenie (cel ma 50% szansy na pominięcie swojej tury)

Woda: słaby przeciwko elektryczność, skuteczny przeciwkoogień. 

Obrażenia cięte: typ obrażeń, Efekt: Krwawienie (kolejne obrażenia tego typu zadają dodatkowe 10%)

Obrażenia obuchowe: Efekt: Ogłuszenie (cel traci kolejną turę, wszystkie ataki na ogłuszony cel są liczone jako krytyczne) 

Uff i to już chyba wszystko. Jeśli potrzebowałabyś sobie czegoś przypomnieć to znajdziesz w swoim ekwipunku moje notatki 

A mając już to wszystko z głowy… 
Skop im tyłek!
~SetVar("Tutorial", 3)
->END

===TutorialEnd===
~SetPortrait("Alice")
Widzisz wcale nie było tak źle
~SetPortrait("Fex")
Nie było tak źle???
Kompletnie ich zniszczyłaś 
Dzisiejsze polowanie pójdzie jak spłatka
~SetPortrait("")
!
~ShowBackground("meteor")
~SetPortrait("Alice")
Też to widzisz?!
...
~HideBackground()
~SetPortrait("")
[Huk]
~SetPortrait("Alice")
To spadło chyba gdzieś w okolicy starych kopalni
Choć musimy to sprawdzić!
~SetPortrait("Fex")
A co jeśli to coś niebezpiecznego?
~SetPortrait("Alice")
To tym lepiej, może to jakiś nieznany dotąd potwór!
Zostaniemy legendami!
~SetPortrait("Fex")
Zdecydowanie nie o to mi chodziło!
~SetPortrait("Alice")
Choć bo to coś jeszcze ucieknie!
->END

