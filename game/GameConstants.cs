using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script
{
    public class GameConstants
    {
        public const string TAG_PLAYER_OBJECT = "PlayerObject";
        public const string TAG_MOB_OBJECT = "MobObject";
        public const string TAG_NPC_OBJECT = "NpcObject";
        public const string TAG_ITEM_OBJECT = "ItemObject";

        public const string TAG_PLAYER_DAMAGE_SOURCE = "PlayerDamageSource";
        public const string TAG_PLAYER_INDIRECT_DAMAGE_SOURCE = "PlayerIndirectDamageSource";

        public const string TAG_MOB_DAMAGE_SOURCE = "MobDamageSource";
        public const string TAG_MOB_INDIRECT_DAMAGE_SOURCE = "MobIndirectDamageSource";

        public const string TAG_NPC_DAMAGE_SOURCE = "NpcDamageSource";
        public const string TAG_NPC_INDIRECT_DAMAGE_SOURCE = "NpcIndirectDamageSource";

        public const string TAG_STATIC_DAMAGE_SOURCE = "StaticDamageSource";
        public const string TAG_STATIC_INDIRECT_DAMAGE_SOURCE = "StaticIndirectDamageSource";

        public const string TAG_REPEL_SOURCE = "RepelSource";
        public const string TAG_INDIRECT_REPEL_SOURCE = "IndirectRepelSource";

        public const string TAG_MAIN_CAMERA = "MainCamera";

        public const string NAME_PLAYER = "player";
        public const string NAME_PLAYER_COLLIDER = "player_collider";
        public const string NAME_PLAYER_CAMERA = "player_camera";
        public const string NAME_PLAYER_CAMERA_TARGET = "player_camera_target";
        public const string NAME_GAME_SCENE_DATA = "scene_data";

        public const string DIRECTORY_FONT = "font/game_font";

        public const int LAYER_PLAYER = 8;
        public const int LAYER_PLAYER_ONLY = 9;
        public const int LAYER_MOB = 10;
        public const int LAYER_MOB_ONLY = 11;
        public const int LAYER_NPC = 12;
        public const int LAYER_NPC_ONLY = 13;
        public const int LAYER_ITEM = 14;
        public const int LAYER_ITEM_ONLY = 15;

        public const int LAYER_WORLD_STATIC = 18;
        public const int LAYER_WORLD_DYNAMIC = 19;
        public const int LAYER_WORLD_STATIC_IGNORE_CAMERA = 20;
        public const int LAYER_WORLD_DYNAMIC_IGNORE_CAMERA = 21;
        public const int LAYER_WORLD_WATER = 22;
        public const int LAYER_WORLD_SCENERY = 23;

        public const int MASK_EVERYTHING = ~0;
        public const int MASK_ONLY_PLAYER = 1 << LAYER_PLAYER;
        public const int MASK_ALL_BUT_PLAYER = ~(1 << LAYER_PLAYER);
        public const int MASK_PLAYER_IGNORES = 
            ~(
                (1 << LAYER_PLAYER)   |
                (1 << LAYER_MOB)      |
                (1 << LAYER_NPC)      |
                (1 << LAYER_ITEM)     |
                (1 << LAYER_MOB_ONLY) |
                (1 << LAYER_NPC_ONLY) |
                (1 << LAYER_ITEM_ONLY)
            );
        public const int MASK_NPC_IGNORES =
            ~(
                (1 << LAYER_PLAYER) |
                (1 << LAYER_MOB) |
                (1 << LAYER_ITEM) |
                (1 << LAYER_PLAYER_ONLY) |
                (1 << LAYER_MOB_ONLY) |
                (1 << LAYER_ITEM_ONLY)
            );
        public const int MASK_MOB_IGNORES =
            ~(
                (1 << LAYER_PLAYER) |
                (1 << LAYER_NPC) |
                (1 << LAYER_ITEM) |
                (1 << LAYER_PLAYER_ONLY) |
                (1 << LAYER_NPC_ONLY) |
                (1 << LAYER_ITEM_ONLY)
            );
        public const int MASK_CAMERA_IGNORES =
            ~(
                (1 << LAYER_PLAYER) |
                (1 << LAYER_MOB) |
                (1 << LAYER_NPC) |
                (1 << LAYER_ITEM) |
                (1 << LAYER_WORLD_STATIC_IGNORE_CAMERA) |
                (1 << LAYER_WORLD_DYNAMIC_IGNORE_CAMERA) |
                (1 << LAYER_PLAYER_ONLY) |
                (1 << LAYER_MOB_ONLY) |
                (1 << LAYER_NPC_ONLY) |
                (1 << LAYER_ITEM_ONLY)
            );

        // event type.

        public const string EVENT_TYPE_NULL = "null";
        public const string EVENT_TYPE_DELAY = "delay";
        public const string EVENT_TYPE_MESSAGE_BOX = "message_box";
        public const string EVENT_TYPE_MESSAGE_BOX_QUESTION = "message_box_question";
        public const string EVENT_TYPE_SET_CAMERA = "set_camera";
        public const string EVENT_TYPE_SET_PLAYER_ANIMATOR = "set_player_animator";
        public const string EVENT_TYPE_SET_PLAYER_EYE = "set_player_eye";
        public const string EVENT_TYPE_UNSET_PLAYER_EYE = "unset_player_eye";
        public const string EVENT_TYPE_SET_ACTOR_ANIMATOR = "set_actor_animator";
        public const string EVENT_TYPE_UNSET_CAMERA = "unset_camera";
        public const string EVENT_TYPE_SAVE_GAME = "save_game";
        public const string EVENT_TYPE_CONDITIONAL_INT = "conditional_int";
        public const string EVENT_TYPE_PLAY_MUSIC = "play_music";
        public const string EVENT_TYPE_PLAY_SOUND = "play_sound";
        public const string EVENT_TYPE_SET_PLAYER_ABILITY = "set_player_ability";
        public const string EVENT_TYPE_SET_OBJECT_ACTIVE = "set_object_active";
        public const string EVENT_TYPE_SET_PLAYER_FACE_DIRECTION = "set_player_face_direction";
        public const string EVENT_TYPE_UNSET_PLAYER_FACE_DIRECTION = "unset_player_face_direction";
        public const string EVENT_TYPE_SET_GAME_VAR_BOOL = "set_game_var_bool";
        public const string EVENT_TYPE_MOVE_OBJECT_POSITION = "move_object_position";
        public const string EVENT_TYPE_CONDITIONAL_BOOL = "conditional_bool";
        public const string EVENT_TYPE_SET_GAME_VAR_INT = "set_game_var_int";
        public const string EVENT_TYPE_RANDOM_NEXT_EVENT = "random_next_event";
        public const string EVENT_TYPE_LOAD_SCENE = "load_scene";
        public const string EVENT_TYPE_STOP_SOUND = "stop_sound";
        public const string EVENT_TYPE_TRIGGER_GAME_EVENT = "trigger_game_event";
        public const string EVENT_TYPE_MOVE_PLAYER_POSITION = "move_player_position";
        public const string EVENT_TYPE_SET_GAME_VAR_STRING = "set_game_var_string";

        // ground type.

        public const string GROUND_TYPE_DEFAULT = "default";
        public const string GROUND_TYPE_GRASS = "grass";
        public const string GROUND_TYPE_SAND = "sand";
        public const string GROUND_TYPE_MUD = "mud";
        public const string GROUND_TYPE_FOLIAGE = "foliage";
        public const string GROUND_TYPE_METAL = "metal";
        public const string GROUND_TYPE_STONE = "stone";
        public const string GROUND_TYPE_WOOD = "wood";
        public const string GROUND_TYPE_WATER = "water";

        // player constants.

        public const string PLAYER_STATE_ATTACK = "attack";
        public const string PLAYER_STATE_CROUCH = "crouch";
        public const string PLAYER_STATE_HIGH_JUMP = "high_jump";
        public const string PLAYER_STATE_HURT = "hurt";
        public const string PLAYER_STATE_DEFAULT = "default";
        public const string PLAYER_STATE_DIVE = "dive";
        public const string PLAYER_STATE_FLUTTER = "flutter";
        public const string PLAYER_STATE_JUMP = "jump";
        public const string PLAYER_STATE_REPEL = "repel";
        public const string PLAYER_STATE_SHOOT = "shoot";
        public const string PLAYER_STATE_WATER_DEFAULT = "water_default";
        public const string PLAYER_STATE_SWIM = "swim";
        public const string PLAYER_STATE_WATER_JUMP = "water_jump";
        public const string PLAYER_STATE_DIE = "die";
        public const string PLAYER_STATE_SLAM = "slam";

        public const string PLAYER_BEHAVIOUR_MOVING_OBJECT = "moving_object";
        public const string PLAYER_BEHAVIOUR_DAMAGE = "damage";
        public const string PLAYER_BEHAVIOUR_REPEL = "repel";
        public const string PLAYER_BEHAVIOUR_WATER = "water";
        public const string PLAYER_BEHAVIOUR_INTERACT = "interact";
        public const string PLAYER_BEHAVIOUR_OXYGEN = "oxygen";

        public const string MOB_BEHAVIOUR_RAYCAST_ALERT = "raycast_alert";
        public const string MOB_BEHAVIOUR_GROUND_CHECK = "ground_check";
        public const string MOB_BEHAVIOUR_WALL_CHECK = "wall_check";
        public const string MOB_BEHAVIOUR_DAMAGE = "damage";
        public const string MOB_BEHAVIOUR_WATER = "water";

        // game state.

        public const string GAME_STATE_GAME = "game";
        public const string GAME_STATE_GAME_OVER = "game_over";
        public const string GAME_STATE_CUTSCENE = "cutscene";
        public const string GAME_STATE_LOAD = "load";
        public const string GAME_STATE_MENU_MAIN = "menu_main";
        public const string GAME_STATE_MENU_PAUSE = "menu_pause";
        public const string GAME_STATE_MENU_SETTINGS = "menu_settings";

        // load setup mode.

        public const string LOAD_SETUP_MODE_MENU = "load_scene_mode_default";
        public const string LOAD_SETUP_MODE_GAME = "load_scene_mode_game";

        // item type.

        public const string ITEM_TYPE_PRIMARY = "primary";
        public const string ITEM_TYPE_SECONDARY = "secondary";
        

        public enum CameraMode
        {
            camera_default,
            camera_fixed,
            camera_fixed_tracking
        }
    }

    

}
