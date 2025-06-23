EXTERNAL AddItem(name)
EXTERNAL PlayCutscene(name)
EXTERNAL SetVar(name, value)
EXTERNAL GetVar(name)
#using AddItem
#using PlayCutscene
#using SetVar
#using GetVar
~AddItem("Lunchbox")

{GetVar("lumberjackTalk") == -1:
    ->NotTaled
  - else:
    ->Talked
}



===NotTaled===
Znalazłeś śniadaniówke drwala z waszej wioski!
Pewnie ją zgubił
Na pewno się ucieszy jeśli mu ją przyniesiesz
Pracuje teraz pewnie gdzieś w lesie
->END

===Talked===
Znalazłeś śniadaniówke drawala!
Z pewnością się ucieszy jeśli mu ją przyniesiesz
Czy wrócić się do drwala?
*[Tak]
~PlayCutscene("Lunchbox")
->END
*[Nie]
->END