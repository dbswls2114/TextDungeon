﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TectDungeon_Skill;

namespace TextDungeon
{

    // 스테이지의 흐름
    // 1. 각각 해당하는 스테이지 메서드를 만든다. 몬스터 스테이지, 특수 스테이지(보스, 랜덤기도, 회복샘,  상점) 1 - 1, 1 - 2 등 1 - n 에 해당하는 세부 적인 장면 구현
    // 2. 스테이지 (3.)에 세팅할 메서드를 만든다. (세부 구현한 스테이지 들을 어떤식으로 배치할지, 몬스터의 대한 매개변수, 아티템에 대한 매개변수를 어떻게 받아올지)
    // 3. 큰 스테이지를 만든다. ( 세부 스테이지에 스테이지를 세팅하는( 2. ) 메서드를 넣어주고 세부적인 스테이지를 넣어준다. ) 


    public class SkillManager
    //SkillManager 클래스는 Skill 객체들을 담는 skills 리스트를 가지고 있다.
    {
        private List<Skill> skills;
        // Skill 객체들을 담는 리스트

        public SkillManager()
        {
            skills = new List<Skill>
                 // SkillManager 생성자, 기본적인 스킬들을 초기화
                {
                 new FireballSkill(),
                 new PowerStrikeSkill(),
                 new SuperPunchSkill()
                 // 기본적인 스킬들을 생성하여 리스트에 추가
                };
        }

        public void ShowSkills()
        // 사용 가능한 스킬 목록을 보여주는 메소드
        {
            Console.WriteLine("어떤 스킬을 사용할까요?");
            for (int i = 0; i < skills.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {skills[i].Name}");
                // 리스트에 있는 각 스킬의 이름과 번호를 출력
            }
        }

        public Skill ChooseSkill(int index)
        // 선택한 인덱스에 해당하는 스킬을 반환하는 메소드
        {
            if (index >= 0 && index < skills.Count)
            {
                return skills[index];
                // 해당 인덱스에 위치한 스킬을 반환
            }
            return null;
            // 옳지 않은 인덱스인 경우, null을 반환
        }

        public int GetSkillsCount()
        // 보유한 스킬의 개수를 반환하는 메소드
        {
            return skills.Count;
            // 보유한 스킬의 개수를 반환
        }
    }


    public class Stage
    {
        List<string> stage1mapname = new List<string>() { "들판", "들판안쪽", "평야외곽", "호수로가는길", "호수", "호수안쪽", "호수바닥" };
        List<string> stage2mapname = new List<string>() { "가위", "바위", "보", "가나다", "라마바", "사아자", "차카" };
        List<string> stage3mapname = new List<string>() { "가위", "바위", "보", "가나다", "라마바", "사아자", "차카" };

        public int thisstagenum = 1;

        public static int stageNum = Program.stageNum;
        public static int minStage = Program.minStage;
        int mapListIndex = 0;
        int stageMapName = 1;

        public static int stageExp = 0;
        public static int stageGold = 0;

        //스테이지 클래스 내부의 전투 메서드 중 플레이어의 턴 부분
        //투하는 부분에서 플레이어의 공격 시에 치명타 확률을 고려하여
        //랜덤하게 치명타가 발생하면 공격력을 두 배로 적용


        int CheckValidInput(int min, int max)
        {
            int keyInput;
            bool result;
            do
            {
                result = int.TryParse(Console.ReadLine(), out keyInput);
            } while (result == false || CheckIfValid(keyInput, min, max) == false);

            return keyInput;
        }

        bool CheckIfValid(int checkable, int min, int max)
        {
            if (min <= checkable && checkable <= max) return true;
            return false;
        }

        public void Stages(Character _player, List<Monster> monsters, int stagenum , int minstage) // 함수의 이름만 바꿔서 큰 스테이지만큼 만들면 댐..
        {
            // 스테이지의 이름 설정을 하기위한 리스트
            mapListIndex = minStage - 1;
            StageSet(_player, monsters, stagenum, minstage); // 스테이지 1에 맞는 세팅을 해주는 메서드.
            if (stageNum == 1)
            {
                BattleScene(_player, monsters, stage1mapname);
            }
            else if (stageNum == 2)
            {
                BattleScene(_player, monsters, stage2mapname);
            }
            else if (stageNum == 3)
            {
                BattleScene(_player, monsters, stage3mapname);
            }
            

        }

        public void StageSet(Character _player, List<Monster> monsters, int stagenum, int minstage) // 스테이지에 따라 몬스터를 다르게 배치할 수 있도록, 여기서 세팅한다.
        {
            stageExp = 0;
            stageGold = 0;
            int randomIncount = new Random().Next(1, 4); // 각 스테이지의 몬스터 수를 랜덤하게 설정한다.
            int randomIncount2 = new Random().Next(1, 2); // 각 스테이지의 몬스터 수를 랜덤하게 설정한다.


            monsters.Clear();
            switch (stagenum)
            {
                case 1:
                    switch (minstage)
                    {
                        case 1:
                            //만약 1-1 방이라면~ 몬스터 세팅을 어떻게 
                            for (int i = 0; i < randomIncount; i++)
                            {
                                monsters.Add(new Slime());
                                stageExp += monsters[i].RExp;
                                stageGold += monsters[i].RGold;
                            } // 스테이지 마다 분리해서 
                            break;
                        case 2: // 1 - 2
                            for (int i = 0; i < randomIncount2; i++)
                            {
                                monsters.Add(new Slime());
                                stageExp += monsters[i].RExp;
                                stageGold += monsters[i].RGold;
                            }
                            for (int i = 0; i < randomIncount2; i++)
                            {
                                monsters.Add(new Goblin());
                                stageExp += monsters[i].RExp;
                                stageGold += monsters[i].RGold;
                            } 
                            break;
                        case 3: // 1 - 3
                            for (int i = 0; i < randomIncount2; i++)
                            {
                                monsters.Add(new Slime());
                                stageExp += monsters[i].RExp;
                                stageGold += monsters[i].RGold;
                            } 
                            for (int i = 0; i < randomIncount2; i++)
                            {
                                monsters.Add(new Goblin());
                                stageExp += monsters[i].RExp;
                                stageGold += monsters[i].RGold;
                            } 
                            break;
                        case 4: // 1 - 4
                            for (int i = 0; i < randomIncount; i++)
                            {
                                monsters.Add(new Goblin());
                                stageExp += monsters[i].RExp;
                                stageGold += monsters[i].RGold;
                            }
                            break;
                        case 5: // 1 - 5                      
                            for (int i = 0; i < randomIncount2; i++)
                            {
                                monsters.Add(new Goblin());
                                stageExp += monsters[i].RExp;
                                stageGold += monsters[i].RGold;
                            }
                            for (int i = 0; i < randomIncount2; i++)
                            {
                                monsters.Add(new Orc());
                                stageExp += monsters[i].RExp;
                                stageGold += monsters[i].RGold;
                            }
                            break;
                        case 6: // 1 - 6                    
                            for (int i = 0; i < randomIncount; i++)
                            {
                                monsters.Add(new Orc());
                                stageExp += monsters[i].RExp;
                                stageGold += monsters[i].RGold;
                            } 
                            break;
                        case 7:// 1 - 7
                            monsters.Add(new GoblinLord());
                            stageExp += monsters[0].RExp;
                            stageGold += monsters[0].RGold;
                            for (int i = 0; i < randomIncount2; i++)
                            {
                                monsters.Add(new Goblin());
                                stageExp += monsters[i].RExp;
                                stageGold += monsters[i].RGold;
                            }
                            break;
                    }
                    break;
                case 2:
                    switch (minstage)
                    {
                        case 1:
                            for (int i = 0; i < randomIncount; i++)
                            {
                                monsters.Add(new SkeletonWarrior());
                                stageExp += monsters[i].RExp;
                                stageGold += monsters[i].RGold;
                            }
                            break;
                        case 2: // 2 - 2
                            for (int i = 0; i < randomIncount2; i++)
                            {
                                monsters.Add(new SkeletonWarrior());
                                stageExp += monsters[i].RExp;
                                stageGold += monsters[i].RGold;
                            }
                            for (int i = 0; i < randomIncount2; i++)
                            {
                                monsters.Add(new SkeletonArcher());
                                stageExp += monsters[i].RExp;
                                stageGold += monsters[i].RGold;
                            }
                            break;
                        case 3:
                            for (int i = 0; i < randomIncount2; i++)
                            {
                                monsters.Add(new SkeletonWarrior());
                                stageExp += monsters[i].RExp;
                                stageGold += monsters[i].RGold;
                            }
                            for (int i = 0; i < randomIncount2; i++)
                            {
                                monsters.Add(new SkeletonArcher());
                                stageExp += monsters[i].RExp;
                                stageGold += monsters[i].RGold;
                            }
                            break;
                        case 4:                           
                            for (int i = 0; i < randomIncount; i++)
                            {
                                monsters.Add(new SkeletonArcher());
                                stageExp += monsters[i].RExp;
                                stageGold += monsters[i].RGold;
                            }
                            break;
                        case 5:
                            for (int i = 0; i < randomIncount2; i++)
                            {
                                monsters.Add(new SkeletonWarrior());
                                stageExp += monsters[i].RExp;
                                stageGold += monsters[i].RGold;
                            }
                            for (int i = 0; i < randomIncount2; i++)
                            {
                                monsters.Add(new Dullahan());
                                stageExp += monsters[i].RExp;
                                stageGold += monsters[i].RGold;
                            }
                            break;
                        case 6:                       
                            for (int i = 0; i < randomIncount; i++)
                            {
                                monsters.Add(new Dullahan());
                                stageExp += monsters[i].RExp;
                                stageGold += monsters[i].RGold;
                            }
                            break;
                        case 7:
                            monsters.Add(new Lich());
                            monsters.Add(new SkeletonWarrior());
                            monsters.Add(new SkeletonArcher());
                            stageExp += monsters[0].RExp;
                            stageGold += monsters[0].RGold;
                            stageExp += monsters[1].RExp;
                            stageGold += monsters[1].RGold;
                            stageExp += monsters[2].RExp;
                            stageGold += monsters[2].RGold;


                            break;
                    }
                    break;
                case 3:
                    switch (minstage)
                    {
                        case 1: // 1 - 1
                            for (int i = 0; i < randomIncount; i++)
                            {
                                monsters.Add(new Drake());
                                stageExp += monsters[i].RExp;
                                stageGold += monsters[i].RGold;
                            }
                            break;
                        case 2: // 1 - 2
                            for (int i = 0; i < randomIncount2; i++)
                            {
                                monsters.Add(new Drake());
                                stageExp += monsters[i].RExp;
                                stageGold += monsters[i].RGold;
                            }
                            for (int i = 0; i < randomIncount2; i++)
                            {
                                monsters.Add(new Wyvern());
                                stageExp += monsters[i].RExp;
                                stageGold += monsters[i].RGold;
                            }
                            break;
                        case 3: // 1 - 3                             
                            for (int i = 0; i < randomIncount2; i++)
                            {
                                monsters.Add(new Drake());
                                stageExp += monsters[i].RExp;
                                stageGold += monsters[i].RGold;
                            }
                            for (int i = 0; i < randomIncount2; i++)
                            {
                                monsters.Add(new Wyvern());
                                stageExp += monsters[i].RExp;
                                stageGold += monsters[i].RGold;
                            }
                            break;
                        case 4: // 1 - 4                        
                            for (int i = 0; i < randomIncount; i++)
                            {
                                monsters.Add(new Wyvern());
                                stageExp += monsters[i].RExp;
                                stageGold += monsters[i].RGold;
                            }
                            break;
                        case 5: // 1 - 5                       
                            for (int i = 0; i < randomIncount2; i++)
                            {
                                monsters.Add(new Wyvern());
                                stageExp += monsters[i].RExp;
                                stageGold += monsters[i].RGold;
                            }
                            for (int i = 0; i < randomIncount2; i++)
                            {
                                monsters.Add(new Griffin());
                                stageExp += monsters[i].RExp;
                                stageGold += monsters[i].RGold;
                            }
                            break;
                        case 6:// 1 - 6
                            for (int i = 0; i < randomIncount; i++)
                            {
                                monsters.Add(new Griffin());
                                stageExp += monsters[i].RExp;
                                stageGold += monsters[i].RGold;
                            }
                            break;
                        case 7:// 1 - 7
                            monsters.Add(new Dragon());
                            stageExp += monsters[0].RExp;
                            stageGold += monsters[0].RGold;
                            break;
                    }
                    break;
            }
        }

        public static void TempleStage(Character _player)
        {
            Console.Clear();

            Console.WriteLine("\n(이계의 )신전을 마주하였습니다.\n");
            Console.WriteLine("   `,.      .   .        *   .    .      .  _    ..          .\r\n     \\,~-.         *           .    .       ))       *    .\r\n          \\ *          .   .   |    *  . .  ~    .      .  .  ,\r\n ,           `-.  .            :               *           ,-\r\n  -             `-.        *._/_\\_.       .       .   ,-'\r\n  -                 `-_.,     |n|     .      .       ;\r\n    -                    \\ ._/_,_\\_.  .          . ,'         ,\r\n     -                    `-.|.n.|      .   ,-.__,'         -\r\n      -                   ._/_,_,_\\_.    ,-'              -\r\n      -                     |..n..|-`'-'                -\r\n       -                 ._/_,_,_,_\\_.                 -\r\n         -               ,-|...n...|                  -\r\n           -         ,-'._/_,_,_,_,_\\_.              -\r\n             -  ,-=-'     |....n....|              -\r\n              -;       ._/_,_,_,_,_,_\\_.         -\r\n             ,-          |.....n.....|          -\r\n           ,;         ._/_,_,_,_,_,_,_\\_.         -\r\n  `,  '.  `.  \".  `,  '.| n   ,-.   n |  \",  `.  `,  '.  `,  ',\r\n,.:;..;;..;;.,:;,.;:,o__|__o !.|.! o__|__o;,.:;.,;;,,:;,.:;,;;:\r\n ][  ][  ][  ][  ][  |_i_i_H_|_|_|_H_i_i_|  ][  ][  ][  ][  ][\r\n                     |     //=====\\\\     |\r\n                     |____//=======\\\\____|\r\n                         //=========\\\\");
            Console.WriteLine("\n원하는 행동을 입력하세요.");
            Console.WriteLine("1. 기도를 올린다.(- 500 G)");
            Console.WriteLine("0. 지나간다.");
            int _playerinput = int.Parse(Console.ReadLine());
            switch (_playerinput)
            {
                case 0:
                    Console.WriteLine("신전을 무시하고 지나갑니다.");
                    //다음 스테이지 연결
                    break;
                case 1:
                    Console.WriteLine("기도를 올렸습니다.");
                    _player.Gold = _player.Gold - 500;
                    Console.WriteLine($"현재 소지금 : {_player.Gold}");
                    //1. 랜덤 스텟 상승 or 감소
                    //2. 랜덤 아이템 획득
                    //다음 스테이지 연결
                    break;
            }
        }//TempleStage()
        // 스테이지 별로 만든다. 보상 스테이지, 몬스터 스테이지, 보스 스테이지



        public void BattleScene(Character _player, List<Monster> monsters, List<string> mapList) // 매개변수 로 플레이어와 몬스터의 정보를 가져온다. // 죽지 않고 클리어시 스테이지2로 연결하는거 해야댐   
        {                                                                                // 몬스터 타입을 넣으면 그에 맞는 몬스터가 나옴
            
            // 어떤 요소가 들어와도 작동 할 수 있게.
            do
            {
                Console.Clear();
                Console.WriteLine($" {stageNum} - {minStage} 스테이지 {mapList[mapListIndex]} \n"); // 스테이지 숫자 변경 해야댐.
                Console.WriteLine(" 플레이어 ");
                Console.WriteLine($"이름 : {_player.Name}, 직업 : {_player.Job}, LV : {_player.Level} "); //직업을 표시해줘야 될까? 
                Console.WriteLine($"공격력 : {_player.Atk}, 체력 : {_player.Hp}, 마나 : {_player.Mp}\n ");

                    for (int i = 0; i < monsters.Count; i++)
                    {
                        Console.WriteLine("{0}: {1} ", i + 1, monsters[i].Name);
                        Console.WriteLine($"공격력 : {monsters[i].Atk}, 체력 : {monsters[i].Hp}\n ");
                    }
                Console.WriteLine("플레이어의 턴!!");
                Console.WriteLine("원하는 행동을 골라 보세요!");
                Console.WriteLine("1. 공격 하기");
                Console.WriteLine("2. 스킬 사용");


                static bool IsCriticalHit(double critChance)
                // 치명타 여부를 결정하는 메서드 추가
                {
                    Random random = new Random();
                    // 랜덤한 값을 생성하기 위해 Random 클래스를 이용
                    double randomNumber = random.NextDouble();
                    // 0부터 1 사이의 랜덤한 double 값을 생성
                    return randomNumber <= critChance;
                    // randomNumber가 critChance보다 작거나 같으면 true를 반환하고
                    // 그렇지 않으면 false를 반환
                    // randomNumber가 critChance보다 작거나 같을 때 크리티컬 히트가 발생한다고 판단
                }
                static bool IsEvaded(double evasion)
                // 플레이어가 회피할지 여부를 결정하는 메서드 추가
                {
                    Random random = new Random();
                    double randomNumber = random.NextDouble();
                    // 0부터 1 사이의 랜덤한 double 값을 생성
                    return randomNumber <= evasion;
                    // 랜덤 숫자가 회피율보다 작거나 같으면 회피 발생
                }


                switch (CheckValidInput(1, 2))
                {

                    case 1:
                        Console.WriteLine("원하시는 몬스터를 선택해주세요.");
                        int selectedMonsterIndex = CheckValidInput(1, monsters.Count) - 1; // monsters의 인덱스는 0부터 시작하기 때문에 -1을 했다.
                        if (monsters[selectedMonsterIndex].Hp <= 0)
                        {
                            Console.WriteLine();
                            Console.WriteLine("이미죽음");
                            //Thread.Sleep(1000);
                            continue; // 다시 값을 입력받기 위해 switch문으로 돌아감
                        }
                        Console.WriteLine("{0}를 공격합니다!", monsters[selectedMonsterIndex].Name);
                        bool isCritical = IsCriticalHit(_player.CritChance);
                        int damageDealt = (int)(_player.Atk * _player.CritiDamage);
                        if (isCritical)
                        // 만약 크리티컬 히트가 발생
                        {
                            Console.WriteLine("치명타 공격!");
                            //추가 피해를 계산하고, "치명타 공격!" 메시지를 출력
                            Console.WriteLine("{0}의 피해를 주었습니다.", damageDealt);
                            monsters[selectedMonsterIndex].Hp -= damageDealt;
                        }
                        else
                        {
                            monsters[selectedMonsterIndex].Hp -= _player.Atk;
                            Console.WriteLine("{0}의 피해를 주었습니다.", _player.Atk);
                        }
                        Console.WriteLine("{0}의 남은체력 :{1}", monsters[selectedMonsterIndex].Name, monsters[selectedMonsterIndex].Hp);

                        if (monsters[selectedMonsterIndex].Hp <= 0)
                        {
                            monsters[selectedMonsterIndex].IsDead = true;
                            Console.WriteLine("{0}을 무찔렀습니다!\n", monsters[selectedMonsterIndex].Name);
                           // Thread.Sleep(1000);
                        }

                        break;
                        //스킬
                    case 2:
                        Console.WriteLine("원하시는 몬스터를 선택해주세요.");
                        int selectedSkillIndex = CheckValidInput(1, monsters.Count) - 1; // monsters의 인덱스는 0부터 시작하기 때문에 -1을 했다.

                        if (monsters[selectedSkillIndex].Hp <= 0)
                        {
                            Console.WriteLine();
                            Console.WriteLine("이미죽음");
                            //Thread.Sleep(1000);
                            continue; // 다시 값을 입력받기 위해 switch문으로 돌아감
                        }

                        SkillManager skillManager = new SkillManager();
                        skillManager.ShowSkills();
                        // 사용 가능한 스킬 보여줌
                        int skillChoice = int.Parse(Console.ReadLine()) - 1;
                        if (skillChoice >= 0 && skillChoice < skillManager.GetSkillsCount())
                        // 선택한 스킬이 유효한지 확인하고 해당 스킬을 가져옴
                        {
                            Skill chosenSkill = skillManager.ChooseSkill(skillChoice);
                            chosenSkill.UseSkill(_player, monsters, selectedSkillIndex);
                            Console.WriteLine($"남은 MP: {_player.Mp}\n");
                            //Thread.Sleep(1000);
                        }
                        else if (_player.Mp < 3)
                        {
                            Console.WriteLine("마나가 부족합니다!");
                            //Thread.Sleep(1000);
                        }
                        else
                        {
                            Console.WriteLine("잘못된 선택입니다.");
                            //Thread.Sleep(1000);
                            continue;
                        }

                        if (monsters[selectedSkillIndex].Hp <= 0)
                        {
                            monsters[selectedSkillIndex].IsDead = true;
                            Console.WriteLine("{0}을 무찔렀습니다!\n", monsters[selectedSkillIndex].Name);
                            //Thread.Sleep(1000);
                        }
                        break;
                }
                //몬스터 턴
                foreach (Monster monster in monsters)
                {
                   // Thread.Sleep(1000);
                    if (monster.Hp > 0)
                    {
                        Console.WriteLine("{0}의 턴!!", monster.Name);
                        bool isEvaded = IsEvaded(_player.Evasion);
                        // 플레이어의 회피를 체크

                        if (isEvaded)
                        {
                            Console.WriteLine("플레이어가 공격을 회피했습니다!");
                            // 회피에 성공했을 때 추가적인 행동을 수행하거나 메시지를 출력
                        }
                        else if (!monster.IsDead)
                        {
                            _player.Hp -= monster.Atk;
                            Console.WriteLine($"{monster.Atk}의 피해를 입었습니다\n");
                            Console.WriteLine($"플레이어의 남은 체력 : {_player.Hp}");                       

                            if (_player.Hp <= 0)
                            {
                                _player.IsDead = true;
                                Console.WriteLine("플레이어가 사망하였습니다.");
                                break;
                            }
                        }
                        else
                        {
                            Console.WriteLine($"{monster.Name}은(는) 죽었습니다!");
                        }
                        
                    }
                    else
                    {
                        Console.WriteLine($"{monster.Name}은(는) 죽었습니다!");
                    }
                }
                 //Thread.Sleep(1000);
                Console.Clear();
            }

            while (!_player.IsDead && monsters.Any(monsters => !monsters.IsDead));
            //Any는 LINQ의 기능입니다. 요소가 하나이상이라도 있으면 true을 반환하고, 비어있으면 false를 반환합니다. // monsters의 리스트 에서 isDead가 false인 몬스터가 하나이상 있는지
            // 확인하려는 용도로 사용했습니다. 즉 살아남은 몬스터가 하나이상 존재한다면 true를 다 죽었다면 false를 반환합니다.
            if (monsters.Any(monsters => monsters.IsDead))
            {
                if (_player.MaxExpCount <= _player.ExpCount)
                {
                    Console.WriteLine("레벨이 올랐습니다.");
                    _player.Level++;
                    _player.ExpCount -= _player.MaxExpCount;
                    _player.MaxExpCount *= _player.Level;
                }

                _player.ExpCount += stageExp;
                _player.Gold += stageGold;

                Console.WriteLine("");
                if (minStage == 7)
                {
                    stageNum++;
                    minStage = 0;
                    mapListIndex = 0;
                }
                else
                {
                    mapListIndex++;
                }
                minStage++;
                if (_player.Mp <= 10)
                {
                    _player.Mp += 5;
                }
                else if (_player.Mp >= 10)
                {
                    _player.Mp = 10;
                }
                Stages(_player, monsters, stageNum, minStage);
            }
            else
            {
                stageNum = 1;
                minStage = 1;
                Program.StartMenu();
            }
        }
    }
}

