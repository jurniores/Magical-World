using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantsRPC
{
   public const byte INSTANT_PLAYER = 0;
   public const byte DESTROY_PLAYER_ROOM = 1;
   public const byte ME_DESTROY_ROOM = 2;
   public const byte UPDATE_ROOM = 3;
   public const byte CHANGE_MASTER = 4;
   public const byte INIT_GAME = 5;
   public const byte INIT_GAME_GO = 6;
   public const byte SET_RUNE = 7;

   //Constants Game


   public const byte INSTANT_PLAYER_GAME = 8;
   public const byte INSTANT_ENEMY_GAME = 9;
   public const byte INSTANT_PLAYERS_GAME = 10;

   //Constans MovePlayer
   public const byte MOVIMENT_PLAYER = 11;
   public const byte MOVIMENT_PLAYER_STOP = 12;
   public const byte MOVIMENT_PLAYER_CORRECT_POSITION = 13;

   //Constans player in Game

   public const byte DEMAGE_PLAYER = 14;
   public const byte SKILLBASE_PLAYER = 15;
   public const byte SKILL1_PLAYER = 16;
   public const byte SKILL2_PLAYER = 17;
   public const byte SKILL3_PLAYER = 18;
   public const byte SKILL4_PLAYER = 19;
   public const byte SKILL5_PLAYER = 20;
   public const byte COWNTDOWN_SKILL = 21;
   
   public const byte CONFIRMED_SKILL = 22;
   public const byte CANCELED_SKILL = 23;
   public const byte RECIEVE_CONFIGS_INITIALS = 24;
   public const byte RECIEVE_DEMAGE = 25;
   public const byte DEATH = 26;

   //BOTS
   public const byte BOT_WALK = 40;
   public const byte BOT_RUN = 41;
   public const byte BOT_ATACK = 42;
   public const byte BOT_STOP = 43;
   public const byte BOT_INSTANTIATE = 44;
   public const byte BOT_POS_INITIAL = 45;

   


}
