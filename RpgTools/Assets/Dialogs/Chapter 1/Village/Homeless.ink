EXTERNAL SetPortrait(name)
EXTERNAL HasItem(name)
EXTERNAL AddItem(name)
EXTERNAL RemoveItem(name)
EXTERNAL GetVar(name)
EXTERNAL SetVar(name, value)
#using SetPortrait
#using HasItem
#using RemoveItem
#using AddItem
#using GetVar
#using SetVar

~SetPortrait("Homeless")

{GetVar("homelessHelped") == -1:
    ->Question
  - else:
    ->End
}




===Question===
Witaj panie kierowniku
nie poraczyłbyś biednego…
Oh to ty Fex
Miło cię znowu widzieć
Słuchaj nie miałbyś może znowu przy sobie czegoś do jedzenia?
{HasItem("Sandwich"):
    *[Poczęstuj kanapką]
    ->Yes
    *[Niesty nic nie mam]
    ->No
  - else:
    *[Niestety nic nie mam]
    ->No
}



==No==
Co za szkoda
->End

===Yes===
~RemoveItem("Sandwich")
~SetVar("homelessHelped", 1)
A mniam miam miam
Ach wielkie dzięki
Wiem że to nie dużo ale masz to wzamian
Znalazłem to gdzieś w krzakach, zapisane jest jakimiś dziwnymi symbolami
może ci się na coś przyda
~AddItem("MathNote1")
~SetPortrait("")
Zdobyłeś Matematyczna Notatkę 1!
Od samego patrzenia na nią zaczyna boleć cię głowa
~SetPortrait("Homeless")
->End

===End===
W każdym razie uważaj na siebie
Słyszałem pogłoski o jakiś tajemniczych metalowych potworach kręcących się po okolicy w ostatnim czasie 
->END