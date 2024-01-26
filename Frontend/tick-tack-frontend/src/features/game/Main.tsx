import { tokenStore } from "@/lib/ApiClient";
import { useAuth } from "@/lib/AuthProvider";
import { Avatar, Button, Card, CardBody, CardFooter, CardHeader, Chip, Divider, Input, Spinner } from "@nextui-org/react";
import { Icon24Send } from "@vkontakte/icons";
import clsx from "clsx";
import { observer } from "mobx-react-lite";
import { CSSProperties, PropsWithChildren, useEffect, useMemo, useState } from "react";
import { useParams } from "react-router-dom";
import { toast } from "react-toastify";
import { ChatController, GameState, Player } from "../../store/GameState";
import { JoinModeEnum, useJoinGame, useMakeMove, useSendMessage } from "./gameApi";
import { realtimeConnection, useRealtimeConnectionStatus } from "./realtimeConnection";

const cross = 'X';
const zero = 'O';
const gap = '_';

const symbolMap: Record<string, string> = {
  [cross]: '❌',
  [zero]: '⭕'
}

const Gap: React.FC<{ gameState: GameState, symbol?: string, x: number, y: number }> = ({ symbol, x, y, gameState }) => {
  const { mutate, isPending } = useMakeMove({ gameState });
  return (
    <div className="p-2 h-full w-full" >
      <button
        disabled={isPending}
        onClick={() => mutate({x, y})}
        className="h-full w-full text-3xl rounded-3xl bg-gray-800 opacity-0 hover:opacity-40 focus-visible:opacity-40 transition-all duration-75"
      >
        {isPending ? <Spinner color="success" /> : symbol}
      </button>
    </div>
  )
};

const Symbol: React.FC<{ text: string }> = ({ text }) => (
  <svg
    width="100%"
    height="100%"
    viewBox="0 0 100 100"
    preserveAspectRatio="xMinYMid meet"
    xmlns="http://www.w3.org/2000/svg"
  >
    <text
      x="2"
      y="75"
      fontSize="70"
      fill="white"
      className="select-none"
    >{text}</text>
  </svg>);

interface GameFieldData {
  gameState: GameState
}

export const GameField: React.FC<GameFieldData> = observer(({ gameState }) => {
  const style: CSSProperties = {
    'gridTemplateRows': `repeat(${gameState.sizeY}, 1fr)`,
    'gridTemplateColumns': `repeat(${gameState.sizeX},  1fr)`
  }

  const tiles = gameState.field.map((x, i) => {
    const row = Math.floor(i / gameState.sizeX);
    const col = i % gameState.sizeX;
    return (
      <div
        key={i}
        className={clsx(
          "aspect-square flex justify-center items-center w-full h-full border-gray-400 border-solid",
          row < gameState.sizeY - 1 && 'border-b-4',
          col < gameState.sizeX - 1 && 'border-r-4',
        )}>
        {x === gap ? gameState.canMove && <Gap x={col} y={row} symbol={symbolMap[gameState.localPlayer.playerMark]} gameState={gameState} /> : <Symbol text={symbolMap[x]} />}
      </div>
    )
  });

  return (
    <div className="grid w-full" style={style}>
      {tiles}
    </div>
  )
});

const PlayerInfo: React.FC<{ player?: Player, reverse: boolean }> = observer(({ player, reverse }) => {
  const avatar = player != null
    ? (<Avatar size="lg" showFallback fallback={(<p className="text-3xl select-none">{symbolMap[player.playerMark]}</p>)} />)
    : (<Avatar size="lg" showFallback />)
  return (
    <>
      {avatar}
      <div className={clsx("flex flex-col", reverse && "items-end")}>
        <p className="text-xl font-medium">{player?.name ?? "Ждем оппонента..."}</p>
        {player && <p className="text-l font-medium">{player?.score}⭐</p>}
      </div>
      {player?.isMoving && <Chip className={reverse ? "mr-auto" : "ml-auto"} color="success" variant="shadow">Ходит</Chip>}
    </>
  )
})

const GameCard: React.FC<{ className?: string, game: GameState }> = observer(({ className, game }) => (
  <Card className={clsx(className)}>
    <CardHeader className="pb-2 px-4 flex items-center gap-4">
      <PlayerInfo player={game.host} reverse={false} />
    </CardHeader>
    <Divider />
    <CardBody>
      <GameField gameState={game} />
    </CardBody>
    <Divider />
    <CardFooter className="pt-2 px-4 flex flex-row-reverse items-center gap-4">
      <PlayerInfo player={game.opponent} reverse={true} />
    </CardFooter>
  </Card>
))

const ChatCard: React.FC<{ chatController: ChatController, gameState: GameState, className?: string }> = observer(({ className, chatController, gameState }) => {
  const { mutate: sendMessage, isPending } = useSendMessage({ chatController, gameState });
  const [msg, setMsg] = useState("");

  function onEnter() {
    if (msg == "") {
      return;
    }
    sendMessage({ message: msg })
    setMsg("")
  }

  return (
    <Card className={clsx(className)}>
      <CardHeader className="flex justify-between">
        <h2 className="text-2xl font-semibold">Чат матча</h2>
        <Button color="danger">Выйти</Button>
      </CardHeader>
      <CardBody>
        {chatController.messages.map(x => (<div><p>{x.sender.username}</p> написал {x.message}</div>))}
      </CardBody>
      <Divider />
      <CardFooter>
        <Input
          variant="flat"
          placeholder="Ваше сообщение"
          value={msg}
          endContent={<Icon24Send />}
          disabled={isPending}
          type={"text"}
          onChange={x => setMsg(x.target.value)}
          onKeyDown={(x) => x.key === 'Enter' && onEnter()}
        />
      </CardFooter>
    </Card>
  );
});

let connecting = false;

const RealtimeManager: React.FC<PropsWithChildren & { game: GameState | null, chat: ChatController }> = observer(({ game, children, chat }) => {
  const [state, stateChanged, failed] = useRealtimeConnectionStatus();
  const { user } = useAuth();

  useEffect(() => {
    if (!user || !game)
      return;

    if (!realtimeConnection.isInitialized && !connecting) {
      connecting = true;
      realtimeConnection.setAccessToken(tokenStore.Token!);
      realtimeConnection.start().then(x => {
        stateChanged();
        connecting = false;
      }).catch(x => {
        toast("Не удалось подклбчиться к игровому серверу")
        failed(x)
      });

      stateChanged();
    }
  }, [user, game])

  useEffect(() => {
    if (!user || !game)
      return;

    const signal = realtimeConnection.connection;
    signal.on("GameStart", game.handleStart.bind(game))
    signal.on("GameOver", game.handleEnd.bind(game))
    signal.on("PlayerMove", game.handleMove.bind(game))
    signal.on("SendMessage", chat.addMessage.bind(chat))
    return () => {
      signal.off("GameStart", game.handleStart)
      signal.off("GameOver", game.handleEnd)
      signal.off("PlayerMove", game.handleMove)
      signal.off("SendMessage", chat.addMessage)
    }
  }, [game])

  return children;
})

export const Main: React.FC<{ guest: boolean }> = observer(({ guest }) => {
  const { id } = useParams<{ id: string }>()
  const { user } = useAuth();
  const [state, stateChanged] = useRealtimeConnectionStatus();

  const [game, setGame] = useState<GameState | null>(null);
  const chatController = useMemo(() => (new ChatController()), []);

  const { mutateAsync: joinGameAsync } = useJoinGame({});

  useEffect(() => {
    (async () => {
      realtimeConnection.setAccessToken(tokenStore.Token!);
      await realtimeConnection.connection.stop();
      stateChanged()

      const res = await joinGameAsync({ gameId: id!, joinMode: guest ? JoinModeEnum.AsViewer : JoinModeEnum.AsPlayer });
      if(game?.gameId != res.gameId){
        setGame(new GameState(user!.userId, 3, 3, guest, res))
      }

      await realtimeConnection.connection.start();
      stateChanged()

      realtimeConnection.connection.invoke('JoinGame', id);
    })().catch((e) => {
      console.log(e);
      toast("Ошибка при подключении", { type: "error" })
      //TODO: redirect
    })

    return () => {
      realtimeConnection.connection.stop();
    }
  }, [id, guest]);

  return (
    <div className="main-game-layout w-full h-full">
      <RealtimeManager game={game} chat={chatController}>
        {
          !state.isConnected && game == null
            ? (<Card className="w-96 self-center row-[2] col-[2]">
              <CardBody>
                <Spinner label="Грузим игру" color="secondary" labelColor="secondary" />
              </CardBody>
            </Card>)
            : (
              <>
                <GameCard game={game!} className="w-96 self-center row-[2] col-[2]" />
                <ChatCard chatController={chatController} gameState={game!} className="w-96 h-full self-center row-[2] col-[3]" />
              </>
            )
        }
      </RealtimeManager>
    </div>
  )
});