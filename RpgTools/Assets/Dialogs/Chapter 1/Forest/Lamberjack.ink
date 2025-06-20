EXTERNAL SetPortrait(name)
EXTERNAL SetVar(name, value)
EXTERNAL GetVar(name)
EXTERNAL HasItem(name)
#using SetPortrait
#using SetVar
#using GetVar
#using HasItem

~SetPortrait("Lumberjack")

{
  -GetVar("lumberjackTalk") == -1:
    ->FirstEncounter
  -GetVar("lumberjackHelped") == 1:
    ->End
  -HasItem("Lunchbox"):
  ->Thanks
  -else:
    ->Brekfast
}



===FirstEncounter===
~SetVar("lumberjackTalk", 1)
Tra-la-la-la, tra-la-la-la
Chop the wood to light the fire
Tra-la-la-la, tra-la-la-la
'Tisn't much that I require
*[Ummm, Przepraszam?]
->FirstTalk
*[...]
->SecondVerse

===SecondVerse===
When the fog of life surrounds you
When you think you've lost your way
Come with me and join the forest
Come with me and join the play
*[Ummm, Przepraszam?]
->FirstTalk
*[...]
->ThirdVerse

===ThirdVerse===
Tra-la-la-la, tra-la-la-la
Youth is such a fragile thing
Tra-la-la-la, tra-la-la-la
A fragile thing is what it is
...
->FirstTalk

===FirstTalk===
Oh, dzień dobry
Nie zauważyłem wcześniej
Długo już tutaj stoisz?
*[Ładnie pan śpiewa]
->Complement
*[Dzień dobry, tylko chwilke]
->FirstTalkB

===Complement===
Oh dziękuje
Takie moje małe hobby
Trzeba sobie jakoś umilać czas w pracy
->FirstTalkB

===FirstTalkB===
W każdym razie co wy tutaj robicie?
Węszycie przez swoim pierwszym polowaniem?
Cóż nie moja sprawa ale uważajcie na siebie
A i jeśli udałoby się wam znaleźć gdzieś samotną śniadaniówke to jest moja
Skubana musiała mi gdzieś wylecieć po drodze 
Jeśli udałoby się wam ją znaleźć to byłym bardzo wdzięczny
{HasItem("Lunchbox"): ->Thanks}
->END

===Brekfast===
Jeśli udałoby się wam natknąć na moją śniadaniówke to byłbym bardzo wdzięczny jeśli byście mi ją przynieśli
->END

===Thanks===
~SetVar("lumberjackHelped", 1)
Oh, widzę że udało się wam ją znaleźć!
Bardzo wam dziękuję!
Macie to za waszą fatygę, z pewnością wam się przyda
~SetPortrait("")
Otrzymaliście Sole trzeźwiące x 5
Przy ich pomocy możecie ocucić przyjaciela kiedy zemdleje w trakcie walki
~SetPortrait("Lumberjack")
Do zobaczenia!
->END

===End===
Jeszcze raz dziękuje wam za pomoc
Do zobaczenia!
->END