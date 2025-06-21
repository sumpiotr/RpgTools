EXTERNAL SetPortrait(name)
EXTERNAL SetVar(name, value)
EXTERNAL GetVar(name)
EXTERNAL ShowBackground(name)
EXTERNAL HideBackground()
EXTERNAL PlayCutscene(name)
#using SetPortrait
#using SetVar
#using GetVar
#using Background
#using PlayCutscene

{
  -GetVar("metalDogEncounter") == -1:
    ->FirstAttack
  -GetVar("metalDogEncounter") == 1:
    ->Danger
  -GetVar("metalDogEncounter") == 2:
  ->End
  -else:
  ->END
}

===FirstAttack===
~SetVar("metalDogEncounter", 1)
~SetPortrait("Alice")
Mój miecz…On po prostu się odbił?
~SetPortrait("WAP")
[Inicjalizacja gotowa w 50%]
->END

===Danger===
~SetVar("metalDogEncounter", 2)
~SetPortrait("Alice")
Nie mamy szans! 
To coś jest niezniszczalne
Uciekaj!
~SetPortrait("")
Widzisz jak Wilkor-R rzuca się Alice do szyi
~SetPortrait("Fex")
Alice!!!
~SetPortrait("")
Bez zastanowienia rzuczasz się pomiędzy nich 
Czas zdaje się jakby spowolnił
~SetPortrait("WAP")
[Inicjalizacja ukończona]
[Wykryto zagrożenie]
[Inicjacja trybu bitewnego]
[Wzmacnianie wydajności organizmu ]
[Przekierowywanie energii do protezy]
[Rozpoczynanie procedurry obronnej]
~ShowBackground("attackScene")
!
~ShowBackground("blue")
!
~HideBackground()
~SetPortrait("Alice")
...
Co się właśnie stało?
...
~SetPortrait("MetalDog")
Grrrrrr
~SetPortrait("Alice")
Ah racja, mniejsza o to 
Pogadamy później
Z tego co widze twój atak uszkodził jego pancerz
Zdaje mi się że powinnam być w stanie coś zrobić
~SetPortrait("")
Nie jesteś do końca pewien co się wydarzyło
Ale czujesz pulsującą w sobie energie
Zdaje ci się że jesteś o wiele silniejszy i szybszy niż wcześniej i jeśli zaszłaby taka potrzeba mógłbyś odtworzyć ten atak
~SetPortrait("WAP")
Odblokowano umiejętność Ostrze Energi! 
Ostrze Energi: typ ataku: Energia, Efekt: pomija obronę przeciwnika, skuteczne przeciwko: metal
~SetPortrait("")
Jest tylko jeden sposób żeby to wszystko przetestować!
~SetPortrait("Fex")
Pomogę ci!
~SetPortrait("")
Fex dołącza do walki!
->END

===End===
~SetVar("metalDogEncounter", 10)
~SetPortrait("MetalDog")
Grhhhrrrrrrr
~SetPortrait("WAP")
[Zakończono analize celu]
[Wykryto kombaytbilny moduł]
[Czy chesz nadpisać ustawienia fabryczne Wilkur-R?]
*[Tak]
[Ustawienia fabryczne Wilkur-R zostały nadpisane]
~SetPortrait("")
Czujesz wieź formującą się pomiędzy tobą a maszyną którą przed chwilą próbowała was zabić
~PlayCutscene("MetalDogTame")
Wilkor-R czea na rozkazy

Szybko, póki się nie rusza, dobijmy go!
~SetPortrait("Fex")
Nie, czekaj!
Siad
~SetPortrait("MetalDog")
[Siada]
~SetPortrait("Fex")
Leżeć
~SetPortrait("MetalDog")
[Kładzie się]
~SetPortrait("Alice")
!!!!
Czy ty to jakoś poskromiłeś?!
Cool!!!
Co w ogóle tutaj właśnie się wydarzyło??
Tworzysz z powietrza jakiś  magiczny miecz, oswajasz potwora i jeszcze do tego potrafisz walczyć
Nic z tego nie ogarniam
~SetPortrait("Fex")
Zdaje mi się że to wszystko może mieć jakiś związek z tym artefaktem który znaleźliśmy 
Wtedy słyszałem ten sam głos
~SetPortrait("Alice")
Głos?!
~SetPortrait("Fex")
No za każdym razem słyszałem jakiś dizwny głos po czym zaczęły dziać się dziwne rzeczyw
~SetPortrait("Alice")
Brzmi to niepokojąco
Zdaje mi się że powinniśmy udać się do mojego taty po pomoc
Na pewno będzie wiedział co zrobić
Ale tak na marginesie pomyśl jakie fory dadzą nam te twoje dziwne umiejetności w naszym teście!
Ciekawe co jeszcze innego potrafisz teraz zrobić!
~SetPortrait("Fex")
...
~SetPortrait("Alice")
Ah racja, wioska, pomoc
Idziemy
->END