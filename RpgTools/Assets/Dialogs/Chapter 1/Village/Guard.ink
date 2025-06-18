EXTERNAL SetPortrait(name)
EXTERNAL GetVar(name)
EXTERNAL PlayCutscene(name)
#using SetPortrait
#using GetVar
#using PlayCutscene


~SetPortrait("Guard")
Stój! 
Z rozkazu wodza nikt nie może opuszczać dzisiaj woski 
Zaczęliśmy zabezpieczanie terenu w związku z dzisiejszą ceremonią i niektóre potwory są jeszcze lekko wkurzone
{GetVar("canLeaveVillage") == -1:
    ->END
  - else:
    Co? Mówisz że szef kazał ci pójść poszukać Alice? 
    Dobrze w takim razie możesz przejść
    ~PlayCutscene("GuardCutscene")
    ->END
}
