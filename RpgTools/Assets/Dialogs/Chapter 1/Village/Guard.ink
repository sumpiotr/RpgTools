EXTERNAL SetPortrait(name)
EXTERNAL GetVar(name)
EXTERNAL SetVar(name, value)
EXTERNAL PlayCutscene(name)
EXTERNAL AddItems(name, count)
#using SetPortrait
#using GetVar
#using SetVar
#using PlayCutscene
#using AddItems

~SetPortrait("Guard")


{GetVar("gateOpened") == -1:
    ->GateClosed
  - else:
    ->GateOpened
}




===GateClosed===
{GetVar("guardFirstTalk") == -1:
    Stój! 
    Z rozkazu wodza nikt nie może opuszczać dzisiaj woski 
    Zaczęliśmy zabezpieczanie terenu w związku z dzisiejszą ceremonią i niektóre potwory są jeszcze lekko wkurzone
    ~SetVar("guardFirstTalk", 1)
  - else:
    To znowu ty?
    Nic się nie zmieniło, brama jest nadal zamknięta
}

{GetVar("canLeaveVillage") == -1:
    ->END
  - else:
    Co? Mówisz że szef kazał ci pójść poszukać Alice? 
    Dobrze w takim razie możesz przejść
    Weź tylko to ze sobą na drogę, może ci się przydać
    ~SetPortrait("")
    ~AddItems("Potion", 5)
    Otrzymałeś 5 mikstur!
    Pozwolą ci przywrócić zdrowie 
    Naciśnij 'I' aby otworzyć swój ekwipunek
    ~PlayCutscene("GuardCutscene")
    ~SetVar("gateOpened", 1)
    ->END
}

===GateOpened===
Droga wolna, skoro szef pozwolił ci wyjść to nie będę cię zatrzymywał
Uważaj na siebie!
->END