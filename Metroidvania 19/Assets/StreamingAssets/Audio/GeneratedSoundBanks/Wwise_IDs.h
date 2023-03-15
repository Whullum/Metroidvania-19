/////////////////////////////////////////////////////////////////////////////////////////////////////
//
// Audiokinetic Wwise generated include file. Do not edit.
//
/////////////////////////////////////////////////////////////////////////////////////////////////////

#ifndef __WWISE_IDS_H__
#define __WWISE_IDS_H__

#include <AK/SoundEngine/Common/AkTypes.h>

namespace AK
{
    namespace EVENTS
    {
        static const AkUniqueID ITEM_PICKUP_STINGER = 1592628004U;
        static const AkUniqueID PLAY_BITE = 1810912708U;
        static const AkUniqueID PLAY_DASH = 2211787386U;
        static const AkUniqueID PLAY_ENEMYDAMAGED_FAUNA = 1068219881U;
        static const AkUniqueID PLAY_ENEMYDAMAGED_METAL = 3777519379U;
        static const AkUniqueID PLAY_INTERACTABLES_BUTTON_LEVER = 1689955131U;
        static const AkUniqueID PLAY_INTERACTABLES_OBJECT_ACTIVATION = 4209986982U;
        static const AkUniqueID PLAY_INTERACTABLES_WALL_DESTRUCTION = 694036599U;
        static const AkUniqueID PLAY_ITEM_PICKUP_STINGER = 2795916357U;
        static const AkUniqueID PLAY_MAINMENU = 3738780720U;
        static const AkUniqueID PLAY_MENU = 1278378707U;
        static const AkUniqueID PLAY_PROJECTILE = 880148599U;
        static const AkUniqueID PLAY_SERPENT_UI_SOUNDS_CANCEL = 3636396448U;
        static const AkUniqueID PLAY_SERPENT_UI_SOUNDS_CONFIRM = 2233317182U;
        static const AkUniqueID PLAY_SERPENT_UI_SOUNDS_HOVER = 1879942652U;
        static const AkUniqueID PLAY_SERPENT_UI_SOUNDS_SELECT = 2449954062U;
        static const AkUniqueID PLAY_STAGE_1 = 2818024140U;
        static const AkUniqueID PLAY_UI_STARTGAME_CONFRIMATION = 1509184699U;
        static const AkUniqueID STOP_MAINMENU = 890527358U;
        static const AkUniqueID STOP_MENU = 2914981333U;
        static const AkUniqueID STOP_STAGEMUSIC = 2611482463U;
    } // namespace EVENTS

    namespace STATES
    {
        namespace GAMEPLAY
        {
            static const AkUniqueID GROUP = 89505537U;

            namespace STATE
            {
                static const AkUniqueID APPROACH_ENEMY = 3438683684U;
                static const AkUniqueID COMBAT = 2764240573U;
                static const AkUniqueID DEATH = 779278001U;
                static const AkUniqueID DEFEAT_ENEMY = 144681581U;
                static const AkUniqueID FINAL_STAGE = 3808459912U;
                static const AkUniqueID LOW_HEALTH = 72790338U;
                static const AkUniqueID NONE = 748895195U;
                static const AkUniqueID NORMAL = 1160234136U;
            } // namespace STATE
        } // namespace GAMEPLAY

        namespace PAUSE
        {
            static const AkUniqueID GROUP = 3092587493U;

            namespace STATE
            {
                static const AkUniqueID NONE = 748895195U;
                static const AkUniqueID PAUSE = 3092587493U;
            } // namespace STATE
        } // namespace PAUSE

    } // namespace STATES

    namespace GAME_PARAMETERS
    {
        static const AkUniqueID ENEMYPROXIMITY = 1850949742U;
        static const AkUniqueID MASTERVOLUME = 2918011349U;
        static const AkUniqueID MUSICVOLUME = 2346531308U;
        static const AkUniqueID SFXVOLUME = 988953028U;
    } // namespace GAME_PARAMETERS

    namespace BANKS
    {
        static const AkUniqueID INIT = 1355168291U;
        static const AkUniqueID MAIN = 3161908922U;
    } // namespace BANKS

    namespace BUSSES
    {
        static const AkUniqueID MASTER_AUDIO_BUS = 3803692087U;
        static const AkUniqueID MUSICBUS = 2886307548U;
        static const AkUniqueID SFXBUS = 3803850708U;
    } // namespace BUSSES

    namespace AUDIO_DEVICES
    {
        static const AkUniqueID NO_OUTPUT = 2317455096U;
        static const AkUniqueID SYSTEM = 3859886410U;
    } // namespace AUDIO_DEVICES

}// namespace AK

#endif // __WWISE_IDS_H__
