EXTERNAL HasItem(name)
EXTERNAL SetVar(name, value)
EXTERNAL GetVar(name)
EXTERNAL SetPortrait(name)
EXTERNAL PlayCutscene(name)
#using SetPortrait
#using HasItem
#using PlayCutscene
#using GetVar
#using SetVar

{
  -GetVar("elevator") == -1:
    ->FirstInteraction
  -HasItem("ElevatorKey"):
    ->UseElevator
  -else:
    ->SecondEncounter
}


===FirstInteraction===
~SetVar("elevator", 1)
~SetPortrait("Alice")
Patrz winda!
Z jej pomocą powinniśmy dostać się do głębin raz dwa
!
Nie działa!
~SetPortrait("Fex")
Zdaje mi się że wymaga jakiegoś klucza do uruchomienia
{HasItem("ElevatorKey"):
    O! Ten który znaleźliśmy wcześniej pasuje
    ->UseElevator
  - else:
   Rozglądnijmy się za nim
}
->END


===SecondEncounter===
Winda
Wymaga klucza do uruchomienia
->END

===UseElevator==
~SetPortrait("")
Czy chcesz skorzystać z windy?
*[Tak]
~PlayCutscene("Elevator1")
->END
*[Nie]
->END