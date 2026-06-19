using Beebyte.Obfuscator;
using System;
using System.Collections.Generic;
using System.Reflection;
[SkipRename]
[Skip]
public class Localize
{

	public static string _banMessage = "Привет читер ты был забанен! Если ты не виноват обратись к администратору игру.";

	public static string[] rare_title = new string[]
	{
		"Дефолт",
		"Эпический",
		"Легендарный",
		"Секретный"
	};

    public static string artefacts_title;

	public static string zombie_winning;

	public static string infection_start;

	public static string people_winning;

	public static string init_nickname;

	public static string common_connect;

	public static string common_cancel;

	public static string mission_gift;

	public static string mission_done;

	public static string mission_nobonus;

	public static string game_fail;

	public static string dialog_play;

	public static string dialog_back;

	public static string dialog_ok;

	public static string postonwall;

	public static string episode1;

	public static string episode2;

	public static string episode3;

	public static string episode4;

	public static string episode5;

	public static string episode6;

	public static string episode7;

	public static string episode8;

	public static string episode_name;

	public static string mm_missions;

	public static string mm_maps;

	public static string mm_online;

	public static string mm_shop;

	public static string mm_bank;

	public static string mm_options;

	public static string mm_invite;

	public static string c_slot;

	public static string c_new_slot;

	public static string c_buy_slot_gold;

	public static string map_loading_data;

	public static string low_gold;

	public static string c_map_title;

	public static string c_map_id;

	public static string c_play_alone;

	public static string c_light_type;

	public static string c_map_choseMap;

	public static string c_map_help;

	public static string common_connect_game;

	public static string password;

	public static string do_load;

	public static string regenerate;

	public static string ys_regenerate;

	public static string no;

	public static string yes;

	public static string ok;

	public static string map_slot_empty;

	public static string create_game;

	public static string dialog_cancel;

	public static string online_server_type;

	public static string players_on_server;

	public static string connect_game;

	public static string onl_type;

	public static string onl_map;

	public static string onl_light;

	public static string onl_break;

	public static string onl_input_password;

	public static string onl_friends;

	public static string onl_empty_rooms;

	public static string onl_unknown_map;

	public static string onl_mission_desc;

	public static string onl_players_online;

	public static string online;

	public static string daily_bonus;

	public static string daily_desc;

	public static string daily_tip;

	public static string take_bonus;

	public static string dont_cheat;

	public static string special_bonus;

	public static string is_one_day;

	public static string is_one_week;

	public static string is_one_mounth;

	public static string is_two_weeks;

	public static string is_unlimit;

	public static string no_money;

	public static string is_reload;

	public static string is_time_left;

	public static string is_days_left;

	public static string is_hours_left;

	public static string is_max;

	public static string is_min_level_to_buy;

	public static string is_ammo_info;

	public static string is_initial_ammo;

	public static string[] is_tabs;

	public static string[] BonusTypeStr;

	public static string vip_info = "Получите статус V.I.P. и у Вас будет:\n1. +{0}% опыта и денег за уровни!\n2. Специальные формы кубов!\n3. Все характеристики на 2 уровня выше!\n4. Возрождение за 10 сек. в режиме выживания!\n5. Возможность разговора по микрофону прямо в игре!\n";

	public static string is_weapon_power;

	public static string money1;

	public static string money2;

	public static string is_upgrade;

	public static string is_weapon_ammo_has;

	public static string is_weapon_ammo_buy;

	public static string is_need_more;

	public static string is_buy_for;

	public static string player_level;

	public static string player_rank;

	public static string player_kills;

	public static string xp_next;

	public static string is_nick;

	public static string is_no_bonus;

	public static string is_no_item;

	public static string mission_developing;

	public static string mission_closed;

	public static string episode_developing;

	public static string[] MissionType = new string[9] { "<empty>", "Найти выход", "Продержаться {0}", "Убить {0} {1} ", "Найти {0}", "Найти {0} за {1}", "Убить {0} {1} за {2}", "Найти выход", "Найти выход за время" };

	public static string[] CubesTypes;

	public static string[] DecorTypes;

	public static string[] ItemsTypes;

	public static string[] WeaponTypes;

	public static string[] CharacterPages;

	public static string[] DeviceTypes;

	public static string[] ClothesType;

	public static string[] monsterName = new string[17]
	{
		"Зомби",
		"Агент",
		"Зомби-солдат",
		"Зомби-шахтёр",
		"Демон",
		"Резак",
		"DEAD-Мороз",
		"Робот 0x1",
		"Робот 0x2",
		"Робот 0x3",
		"Робот 0x4",
		"Робот-Босс 0x5",
		"Агент",
		"Сталкер",
		"Рыцарь",
		"Ракетный солдат",
		string.Empty
	};

	public static string[] AAnames;

	public static string[] triggerTypeName;

	public static string[] triggerConditionActivateName;

	public static string[] triggerNeedKeyName;

	public static string[] gameItemsNames = new string[139]
	{
		"Факел",
		"Ступеньки К",
		"Ступеньки Д",
		"Лестница",
		"Дверь",
		"Стальная дверь",
		"Стальная дверь 2",
		"Двойная д. дверь",
		"Двойная с. дверь",
		"Ворота",
		"Светильник",
		"Люстра",
		"Красный шар",
		"Зелёный шар",
		"Синий шар",
		"Бочка",
		"Железная бочка",
		"Железная бочка 2",
		"Гроб",
		"Забор",
		"Ограда",
		"Ограда 2",
		"Картина",
		"Картина 2",
		"Скамейка",
		"Скамейка 2",
		"Грибы",
		"Папоротник",
		"Ромашки",
		"Трава",
		"Цветочки",
		"Стол 1",
		"Стол 2",
		"Стул",
		"Сундук",
		"Факел",
		"Куб-лампа(бел)",
		"Куб-лампа(син)",
		"Куб-лампа(зел)",
		"Респаун",
		"Респаун Красных",
		"Респаун Синих",
		"Респаун Зелёных",
		"Респаун Золотых",
		"Респаун патронов",
		"Респаун гильз",
		"Респаун ракет",
		"Респаун энергии",
		"Респаун аптечки",
		"Респаун брони",
		"Респаун ружъя",
		"Респаун обреза",
		"Респаун УЗИ",
		"Респаун автомата",
		"Респаун базуки",
		"Респаун ПТУРС",
		"Респаун зомби",
		"Кнопка",
		"Рычаг",
		"Рычаг на стену",
		"Телепортатор",
		"Невидимый выключатель",
		"Респаун зомби",
		"Респаун копа",
		"Респаун солдата",
		"Респаун шахтёра",
		"Респаун демона",
		"Спрятанный предмет",
		"Куб-лампа(жёл)",
		"Куб-лампа(кр)",
		"Кровать",
		"Стальная лестница",
		"Веревка",
		"Стальные ступеньки",
		"Елочка",
		"Снежная елочка",
		"Большая елочка",
		"Квадрик",
		"Трамплин 3х1",
		"Трамплин 3х2",
		"Трамплин 3х3",
		"Респаун турели",
		"Респаун Банши",
		"Красный флаг",
		"Синий флаг",
		"Зелёный флаг",
		"Золотой флаг",
		"Респаун снайперки",
		"Респаун выжигателя",
		"Точка доминирования",
		"Армейский БТР",
		"Респаун Резак",
		"Джампер",
		"Пушер",
		"Комп",
		"Стильный Комп",
		"Зомби Агент",
		"Зомби Сталкер",
		"Ослепляющая граната",
		"Взрыв пакет",
		"Зелёное дерево",
		"Синее дерево",
		"Осеннее дерево",
		"Вырастить стену",
		"Аптечка и броня",
		string.Empty,
		"Запас патронов",
		"Динамит",
		"Мина",
		"Живая вода",
		"Дверь вертикальная",
		"Дверь горизонтальная",
		"Лифтовая шахта",
		"Силовое поле",
		"Обойма патронов",
		"Обойма гильз",
		"Провод",
		"Обойма ракет",
		"Обойма энергии",
		"Мега-запас",
		"Дверь",
		"Комод",
		"Ковер",
		"Шкаф",
		"Стол",
		"Табурет",
		"Свечи",
		"Джип",
		"Ренегат",
		"Робот 0x1",
		"Робот 0x2",
		"Робот 0x3",
		"Робот 0x4",
		"Робот-Босс 0x5",
		"Легкий БТР",
		"Вертолет",
		"Зомби рыцарь",
		"Реактивный солдат",
		"Лодка"
	};

	public static string[] gameItemsDesc = new string[140]
	{
		"Освещает тёмные места",
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		"Огромные ворота для замков",
		"Электрический фонарь на стену",
		"Свечная люстра для потолка",
		"Красиво освещает тёмные места",
		"Красиво освещает тёмные места",
		"Красиво освещает тёмные места",
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		"Факел в землю, освещение больших площадей",
		"Для встраивания в здания",
		"Для встраивания в здания",
		"Для встраивания в здания",
		"Респаун в некомандных режимах.",
		"Респаун красной команды",
		"Респаун синей команды",
		"Респаун зелёной команды",
		"Респаун золотой команды",
		"Появляются в бою через некоторое время",
		"Появляются в бою через некоторое время",
		"Появляются в бою через некоторое время",
		"Появляются в бою через некоторое время",
		"Появляются в бою через некоторое время",
		"Появляются в бою через некоторое время",
		"Появляются в бою через некоторое время",
		"Появляются в бою через некоторое время",
		"Появляются в бою через некоторое время",
		"Появляются в бою через некоторое время",
		"Появляются в бою через некоторое время",
		"Появляются в бою через некоторое время",
		"Монстры появляются в режиме Выживания",
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		"Спрячьте предмет, пусть другие поищут",
		"Для встраивания в здания",
		"Для встраивания в здания",
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		"Светится гирляндами",
		"Светится гирляндами",
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		"Взрыв ослепляет противника",
		"Взрывается при броске",
		"Вырастает зелёное дерево случайного размера",
		"Вырастает синее дерево случайного размера",
		"Вырастает оранжевое дерево случайного размера",
		"Достраивает 4 куба на выбранную грань",
		"Восстанавливает здоровье и броню",
		string.Empty,
		"Восстанавливает патроны и гильзы",
		"Взрывается через 5 секунд",
		"Взрывается при приближении",
		"Воскрешает персонажа в режиме выживания",
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		"300патронов, 100гильз, 50ракет, 300энергии",
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty
	};

	public static string[] specItemsName;

	public static string[] specItemsDesc = new string[9] { "Будет включаться при прыжке", "Танцевать - русск. Р (англ H)", "Танцевать - русск. Р (англ H)", "Танцевать - русск. Р (англ H)", "Танцевать - русск. Р (англ H)", "Танцевать - русск. Р (англ H)", "Танцевать - русск. Р (англ H)", "Танцевать - русск. Р (англ H)", "Танцевать - русск. Р (англ H)" };

	public static string move_learn;

	public static string[] RankName = new string[46]
	{
		"Новобранец", "Младший рекрут", "Рекрут", "Старший рекрут", "Рекрут 1 класса", "Солдат", "Рядовой рекрут", "Рядовой 2 класса", "Рядовой", "Рядовой 1 класса",
		"Специалист", "Старший специалист", "Специалист 1 класса", "Разведчик", "Младший капрал", "Капрал", "Командир", "Сержант 3 класса", "Сержант 2 класса", "Сержант 1 класса",
		"Коммандор-сержант 3кл.", "Коммандор-сержант 2кл", "Коммандор-сержант 1кл", "Коммандор", "Младший офицер", "Старший офицер", "Младший лейтенант", "Второй лейтенант", "Лейтенант", "Капитан",
		"Капитан разведки", "Капитан спецназа", "Подполковник", "Полковник", "Генерал", "Генерал Армии", "Главнокомандующий", "Маршал", "Капитан отряда", "Майор отряда",
		"Генерал-лейтенант", "Полковник отряда", "Генерал-полковник", "Маршал отряда", "Мститель", "Безумец"
	};

	public static string[] findPrefabsNames;

	public static string[] newMapTypeName;

	public static string[] advices = new string[18]
	{
		"Цветные китайские шары светятся просто волшебно в темноте!", "Ракета из ПТУРС управляется мышкой.", "Одежда даёт  бонусы к характеристикам!", "Не забудьте купить патронов для боя!", "Для VIP игроков доступны особые формы кубов.", "F10 - не забывайте сохранять свою карту!", "F10 - не забывайте сохранять свою карту!", "Со статусом V.I.P. все кубы бесплатные", "Со статусом V.I.P. быстрее накапливается уровень и деньги", "Нажмите V для использования голосового чата",
		"Игра, в которой играет друг, сразу отобразится в сетевой битве справа.", "Пройдите по гробу погибшего игрока и он воскреснет", "В режиме выживания опыта за фраг получается в 10 раз меньше, чем в битве.", "Если удалить предмет с карты - он НЕ появится в инвентаре", "Не нравится ваша карта, а новый слот не по карману? Перегенерируйте карту!", "На 5 день игры Вам дадут бонус - 2 золотых.", "Инвентарь(С)-Перс-VIP - будь круче на 2 звезды!", "Инвентарь(С)-Перс-VIP - будь круче на 2 звезды!"
	};

	public static string[] gameTypeStr;

	public static string[] buildinMapName = new string[32]
	{
		"Перекрёстный огонь",
		"Мясной куб",
		"Город",
		"Бассейн",
		"Особняк",
		"Склад",
		"Остров",
		"Зион-Тех",
		"В песках",
		"Зомби-сити",
		"Арена",
		"Бибз",
		"Страйк",
		"Мотель",
		"Мясорубка",
		"Река",
		"Залив",
		"Остров",
		"Плоская",
		"Перекресток",
		"Река",
		"Арена",
		"Две башни",
		"Страйк",
		"Assault Pro",
		"Мотель",
		"Луммокс",
		"Арголис",
		"Арена Sontar",
		"Мясной куб 2",
		"Город алого заката",
		string.Empty
	};

	public static string[] teamName;
	public static string[] teamsInfection = new string[]
	{
		"Зомби","Люди"
	};

	public static string[] graphStrs;

	public static string[] weaponNames;

	public static string[] clothesName;

	public static string[] skinName;

	public static string[] bulletsNames;

	public static string AAS_Position;

	public static string AAS_Upper;

	public static string AAS_Lower;

	public static string AAS_Far;

	public static string AAS_Near;

	public static string AAS_Right;

	public static string AAS_Left;

	public static string AAS_Size;

	public static string AAS_Higher;

	public static string AAS_Smaller;

	public static string AAS_Longer;

	public static string AAS_Shorter;

	public static string AAS_Wider;

	public static string AAS_Uje;

	public static string AAS_Opening_for_N_sec;

	public static string sec;

	public static string AAS_rotation;

	public static string save;

	public static string BCS_cant_save_if_joined;

	public static string BCS_only_owner_can_save;

	public static string BCS_cant_save_in_battle;

	public static string BCS_end_round;

	public static string[] BCS_levelUpStrs;

	public static string BCS_endGame_timeout;

	public static string BCS_endGame_ban;

	public static string BCS_endGame_noSuvivours;

	public static string BCS_endGame_gameOver;

	public static string BCS_endGame_lostConnection;

	public static string BCS_endGame_tryAgain;

	public static string BCS_ban_from_server;

	public static string BCS_disable_miscrophone;

	public static string BCS_to_speak_press;

	public static string BCS_enable_microphone;

	public static string BCS_then_press_allow;

	public static string BCS_get_VIP_to_speak;

	public static string BCS_start_test;

	public static string BCS_resume_game;

	public static string BCS_save_map;

	public static string BCS_inventory_shop;

	public static string BCS_exit_from_game;

	public static string BCS_allowed;

	public static string BCS_notallowed;

	public static string BCS_noobs_to_build;

	public static string BCS_forbidBuild;

	public static string BCS_allowBuild;

	public static string BCS_ban;

	public static string BCS_end_test;

	public static string BCS_name;

	public static string BCS_frags;

	public static string BCS_deathes;

	public static string BCS_wait_while_saving;

	public static string BCS_dont_forget_save_map;

	public static string BCS_save_and_exit;

	public static string BCS_dont_save_exit;

	public static string BCS_new_level;

	public static string BCS_new_rang;

	public static string next;

	public static string BCS_exp;

	public static string BCS_time;

	public static string BCS_frags_per_minute;

	public static string BCS_earned;

	public static string BCS_choose_team;

	public static string score;

	public static string BCS_enter;

	public static string BCS_loading;

	public static string BCS_advice;

	public static string BCS_speak_then_release;

	public static string BCS_speaker_disabled;

	public static string howto_play;

	public static string howto_menu;

	public static string howto_close;

	public static string howto_walk;

	public static string howto_jump;

	public static string howto_placeCube1;

	public static string howto_placeCube2;

	public static string howto_removeCube1;

	public static string howto_removeCube2;

	public static string howto_inventory;

	public static string howto_chooseItem;

	public static string howto_savemap;

	public static string howto_chat;

	public static string howto_voiceChat;

	public static string howto_fullscreen;

	public static string howto_options;

	public static string speedHackDetected;

	public static string[] strTutor = new string[31]
	{
		"Добро пожаловать на полигон, солдат! Твоё обучение будет проходить в обстановке, близкой к боевой! Итак, задание - найти проход к заброшенной лаборатории! Осмотрись, и найди для начала выход из этого грота!",
		"Кажется, это чёрный вход. Эта дверь ведёт к лаборатории. Нажми на кнопку, чтобы открыть дверь.",
		"Столб пыли в конце комнаты - это секретная разработка антигравитационного лифта! Чтобы включить его - нажми кнопку слева от лифта.",
		"Молодец! Мне нравится, как ты справляешься! В конце этого коридора, должна быть дверь - похоже, мы дошли до лаборатории!",
		"Замри! Видишь - в соседней комнате стоит враг - зомби. Возьми кирку и уничтожь врага! (нажмите С)",
		"Боец, ты не перестаёшь меня удивлять! Дальше будет веселее.",
		"Теперь нужно освоить более мощное оружие. Возьми этот автомат.",
		"Оружие можно прокачать в инвентаре, заодно и увеличить количество патрон",
		"Базука... А за дверью толпа зомби.. Ты знаешь, что делать...",
		"Отлично, боец! Давай в телепорт, ты великолепно себя показал!",
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		"А вот и твой мир! Давай, я расскажу, как в нём строить.",
		"Нажми C и выбери любой куб\nЗатем построй 5 кубов в любом месте (правая кнопка мыши)",
		"Молодец! Теперь разрушь 5 любых кубов (левая кнопка мыши)",
		"Молодец! Смотри, наступила ночь! Нужно больше света!",
		"Нажми С - Предметы - купи ФАКЕЛ и поставь на карту",
		"Отлично! Осталось сохранить твою карту! Нажми F10.",
		"Теперь можно построить все что угодно! Желаем приятной стройки!",
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty
	};

	public static string[] miniStrTutor = new string[30]
	{
		"Найди туннель в пещере",
		"Нажать на рычаг, чтобы открыть дверь",
		"Нажать кнопку, чтобы включить лифт",
		"Дойти до лаборатории",
		"Уничтожить зомби киркой",
		"Подняться по ступенькам на поверхность",
		"Взять автомат",
		"Нажать R и купить патронов",
		"Уничтожить всех зомби",
		"Шагнуть в телепорт",
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		"Сохраните карту - F10",
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty
	};

	public static string tutor_need_reload;

	public static string tutor_reload;

	public static string tutor_choose_weapons;

	public static string tutor_choose_bullets;

	public static string tutor_buy_ammo;

	public static string tutor_close_inventory;

	public static string tutor_kill_monsters;

	public static string tutor_placeCube;

	public static string tutor_placeMoreCubes;

	public static string tutor_open_inventory;

	public static string tutor_choose_cubes;

	public static string tutor_choose_nature;

	public static string tutor_choose_any_cube;

	public static string tutor_place_cube_inventar;

	public static string tutor_remove_cube;

	public static string tutor_remove_cubes_more;

	public static string tutor_place_fire;

	public static string tutor_choose_decor;

	public static string tutor_choose_light;

	public static string tutor_choose_fire;

	public static string tutor_buy_one_fire;

	public static string tutor_place_fire_inventar;

	public static string tutor_kill_zombie_and_go;

	public static string tutor_choose_axe;

	public static string tutor_buy_axe_forever;

	public static string tutor_place_axe_inventar;

	public static string short_name;

	public static string rang_table;

	public static string params_health;

	public static string params_armor;

	public static string param_speed;

	public static string param_jump;

	public static string param_defend;

	public static string vip_not_enougth;

	public static string vip_activated;

	public static string bank_name;

	public static string inventory_name;

	public static string maximum;

	public static string hello_chiter;

	public static string server_error;

	public static string ban_cheater;

	public static string loading_data;

	public static string[] newGameMapTypeStr;

	public static string[] filterGameTypeStr;

	public static string[] mapCanBreakStr;

	public static string[] mapDayLightStr;

	public static string[] serverTypeStr;

	public static string begin_game;

	public static string any_map;

	public static string map;

	public static string self_map;

	public static string wrong_password;

	public static string[] respawnTimeStr;

	public static string monster_options;

	public static string monster_type;

	public static string ressurection_time;

	public static string error_empty_map;

	public static string player_can_build_now;

	public static string player_cant_build_now;

	public static string opt_name;

	public static string opt_graph;

	public static string opt_worse;

	public static string opt_better;

	public static string opt_sound;

	public static string opt_silent;

	public static string opt_louder;

	public static string opt_empty_screen;

	public static string opt_smooth_follow;

	public static string opt_close;

	public static string connect_failed;

	public static string create_room_failed;

	public static string put_on_items;

	public static string cube_occupied;

	public static string to_delete_press;

	public static string to_move_press;

	public static string to_activate_press;

	public static string to_rotate_press;

	public static string to_edit_press;

	public static string cant_build_ask_admin;

	public static string cant_change_world;

	public static string not_enougth_cubes;

	public static string beside_world;

	public static string place_on_cube_side;

	public static string place_on_ceil;

	public static string cant_already_remove;

	public static string find_item_choose_type;

	public static string find_item_type;

	public static string apply;

	public static string _for;

	public static string ps_choose_new_item_place;

	public static string ps_press_for_respawn;

	public static string ps_you_dead_try_again;

	public static string ps_survival_dead;

	public static string ps_survival_if_VIP;

	public static string ps_use_vita_water;

	public static string ps_press_for_use_vita_water;

	public static string ps_before_respawn_secs;

	public static string ps_health_and_armor_restored;

	public static string ss_loading;

	public static string social_tell_friends;

	public static string social_menu;

	public static string social_invite_friends;

	public static string social_group;

	public static string social_invite_num_friends;

	public static string social_tell_about_game;

	public static string social_install_game;

	public static string trig_options;

	public static string trig_type;

	public static string trig_triggered_if;

	public static string trig_need_for_triggering;

	public static string trig_mission_exit;

	public static string wire_put_on_switch;

	public static string wire_options;

	public static string wire_signal_delay;

	public static string wire_connected_with;

	public static string wire_not_connected;

	public static string wire_connected_moveable_cubes;

	public static string wire_connected_switch;

	public static string wire_connected_item;

	public static string wire_connected_coords;

	public static string wire_connect_to;

	public static string wire_choose_connect_to;

	public static string plazm_grenade_gun;

	public static string seconds;

	public static string secondu;

	public static string secondy;

	public static string minutes;

	public static string minutu;

	public static string minuty;

	public static string monsters;

	public static string monstra;

	public static string viral_title;

	public static string viral_bonus;

	public static string player_exit;

	public static string player_joined;

	public static string need_bullets_nothing;

	public static string need;

	public static string put_on;

	public static string activities_title;

	public static string[] offer_text = new string[3] { "Дамы и господа, торопитесь только сегодня действует уникальная распродажа. Скидки в магазине на указанные товары 50%", "Выгодная цена на золото", "Больше баксов и опыта на миссиях" };

	public static string monster_health_mult;

	public static string monster_damage_mult;

	public static string monster_state;

	public static string boss;

	public static string newYearBoss_mission_adv;

	public static string newYearBoss_mission_need_level;

	public static string transport_options;

	public static string transport_type;

	public static string to_drive_press;

	public static string no_place_to_drive;

	public static string press_to_end_drive;

	public static string[] mission_name = new string[128];

	public static string shots_per_sec;

	public static string requireBullets;

	public static string for_fullScreen_press;

	public static string pieses;

	public static string Buy;

	public static string social_folow_fanpage;

	public static string you_killed;

	public static string you_infected;

	public static string you_was_killed_by;

	public static string killed;

	public static string to_next_round_last;

	public static string shooter_end_battle;

	public static string[] winner_place;

	public static string dead_by_nature;

	public static string dead_himself;

	public static string dead_by_zombie;

	public static string he_saved;

	public static string you_saved;

	public static string he_saved_you;

	public static string headshot;

	public static string offer_title;

	public static string sale_title;

	public static string offer_open_shop;

	public static string offer_open_bank;

	public static string howto_tab;

	public static string you_have_flag;

	public static string your_flag_captured;

	public static string flag_has_been_captured;

	public static string[] flag_color_name;

	public static string takes_flag;

	public static string dropped_flag;

	public static string flag;

	public static string BCS_Score;

	public static string cant_take_flag_no_players;

	public static string you_returned_flag;

	public static string[] bonusName = new string[42]
	{
		"ПОДРЫВ", "ДВОЙНОЙ ПОДРЫВ", "ВЗРЫВ", "ДВОЙНОЙ ВЗРЫВ", "2 ФРАГА", "5 ФРАГОВ", "7 ФРАГОВ", "10 ФРАГОВ", "ХЭДШОТ", "ХЭДШОТ 3",
		"ХЭДШОТ 5", "СПАСАТЕЛЬ", "5 СПАСЁННЫХ", "СВЯТОЙ", "РУКОПАШКА", "РУКОПАШКА 3", "САМОУБИЙЦА", "КОМАНДА-ПОБЕДИТЕЛЬ", "1 МЕСТО", "2 МЕСТО",
		"3 МЕСТО", "ЗАХВАТ ФЛАГА", "МИССИЯ ПРОЙДЕНА", "5 зомби", "20 зомби", "50 зомби", "3 демона", "10 демонов", "3 зомби взрывом", "20 зомби взрывом",
		"50 зомби взрывом", "Уничтожение техники", "3 техники", "1 волна", "3 волны", "5 волн", "10 волн", "50 кубов", "150 кубов", "5 предметов",
		"20 предметов", "Механизм"
	};

	public static string your_team_is;

	public static string frags_killed;

	public static string need_level;

	public static string[] weapon_upgrade_name = new string[5] { "Урон", "Разброс", "Скорострельность", "Обойма", "Начальный запас" };

	public static string[] DecorTypesNew;

	public static string[] SpawnerTypes;

	public static string no_weapon_upgrade = "Можно прокачать только оружие купленное навсегда";

	public static string needParamsToBuy1;

	public static string needParamsToBuyLevel;

	public static string needParamsUpgradeNow;

	public static string Unlock;

	public static string Upgrade;

	public static string jumper_options;

	public static string jumper_options_height;

	public static string building;

	public static string mapsTop;

	public static string anyBattle;

	public static string withFriends;

	public static string createGame;

	public static string friendsMaps;

	public static string findMap;

	public static string buySlotFor10;

	public static string allMaps;

	public static string addFor10;

	public static string accuracy;

	public static string nearFight;

	public static string lightWeapon;

	public static string shotguns;

	public static string assaults;

	public static string heavy;

	public static string precision;

	public static string equipment;

	public static string weapons;

	public static string dances;

	public static string myClan;

	public static string allClans;

	public static string createFor10;

	public static string createForX;

	public static string edit;

	public static string leave;

	public static string gameType;

	public static string add;

	public static string addMap;

	public static string mapCanBreak1;

	public static string weapon_upgrade_name0;

	public static string weapon_upgrade_name2;

	public static string weapon_upgrade_name3;

	public static string weapon_upgrade_name4;

	public static string is_tabs0;

	public static string tabsItems;

	public static string is_tabs2;

	public static string is_tabs3;

	public static string clanShort;

	public static string clanURL;

	public static string clanName;

	public static string clanJoin;

	public static string clan;

	public static string myMap;

	public static string find;

	public static string clear;

	public static string buyToUpgrade;

	public static string message;

	public static string start;

	public static string buildinMaps;

	public static string music;

	public static string mouseSens;

	public static string sound;

	public static string settings;

	public static string graph;

	public static string resolution;

	public static string activate;

	public static string close;

	public static string character;

	public static string main;

	public static string game;
	public static string arsenal;

	public static string career;

	public static string fastBattle;

	public static string use;

	public static string twiceGoldForFirstDonate;

	public static string myMapsListTutor;

	public static string dialog_next;

	public static string exitGame;

	public static string exit;

	public static string pauseMenu_rank;

	public static string pauseMenu_name;

	public static string pauseMenu_kills;

	public static string pauseMenu_deathes;

	public static string pauseMenu_ban;

	public static string pauseMenu_allow;

	public static string pauseMenu_beginTest;

	public static string pauseMenu_saveMap;

	public static string pauseMenu_healers;

	public static string pauseMenu_gamers;

	public static string pauseMenu_wave;

	public static string pauseMenu_score;

	public static string clan_fail_new;

	public static string clan_url_default;

	public static string need_prew_upgrade;

	public static string instal_android_many;

	public static string ui_buy_for;

	public static string quit_yesno;

	public static string pack;

    public static string wait_players = "Ожидание игроков";

    private static Dictionary<string, string> _hash = new Dictionary<string, string>();

	private static Type _tt = typeof(Localize);


	public static Dictionary<string, string> hash
	{
		get
		{
			return _hash;
		}
	}

	public static string T(string key)
	{
		if (_hash.ContainsKey(key))
		{
			return _hash[key];
		}
		FieldInfo field = _tt.GetField(key);
		if (field == null)
		{
			return key;
		}
		_hash[key] = (string)_tt.GetField(key).GetValue(null);
		return _hash[key];
	}
}
