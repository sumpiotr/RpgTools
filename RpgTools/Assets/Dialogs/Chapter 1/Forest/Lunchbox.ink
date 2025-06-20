EXTERNAL AddItem(name)
EXTERNAL PlayCutscene(name)
#using AddItem
#using PlayCutscene
Znalazłeś śniadaniówke drawala!
Z pewnością się ucieszy jeśli mu ją przyniesieś
Czy wrócić się do drwala?
*[Tak]
~PlayCutscene("Lunchbox")
->END
*[Nie]
->END