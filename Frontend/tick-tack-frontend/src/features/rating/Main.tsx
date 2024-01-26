import { Button, ButtonGroup, Card, CardBody, CardHeader, Divider, Input, Popover, PopoverContent, PopoverTrigger, User } from "@nextui-org/react"
import { Icon24DoorArrowLeftOutline, Icon24View } from "@vkontakte/icons"
import { GetGameListResponse, useCreateGame, useGetGameList } from "./gameListApi"
import { GameInfoDto, GameStateEnum, MarkEnum, PlayerInfoDto } from "./types"
import dayjs from "dayjs";
import { useNavigate } from "react-router-dom";
import { useState } from "react";

function getMark(player: PlayerInfoDto) {
  return player.mark === MarkEnum.Cross ? '❌' : '⭕'
}

const GameJoinButton: React.FC<{ gameInfo: GameInfoDto }> = ({ gameInfo }) => {
  if (gameInfo.state != GameStateEnum.Started && gameInfo.state != GameStateEnum.NotStarted) {
    return null;
  }

  return (
    <ButtonGroup className="mr-1">
      <Button size="md" isIconOnly color="default"><Icon24View /></Button>
      {gameInfo.state === GameStateEnum.NotStarted && <Button size="md" isIconOnly color="secondary"><Icon24DoorArrowLeftOutline /></Button>}
    </ButtonGroup>
  );
}

const GameCard: React.FC<{ gameInfo: GameInfoDto }> = ({ gameInfo }) => (
  <div className="flex items-center justify-between bg-default-100 rounded-lg p-2">
    <User
      name={gameInfo.host.username}
      avatarProps={{
        fallback: (<p className="text-2xl select-none">{getMark(gameInfo.host)}</p>)
      }}
      description={`${gameInfo.host.rating}⭐`}
    />
    <div className="flex flex-col items-center">
      <p className="text-lg font-semibold">&lt;1000⭐</p>
      <p className="text-small text-foreground-500">{dayjs(gameInfo.createdAt).fromNow()}</p>
    </div>
    <GameJoinButton gameInfo={gameInfo} />
  </div>
)

const GameStartedCard: React.FC = () => (
  <div className="flex items-center justify-between bg-default-100 rounded-lg p-2">
    <div className="flex items-center gap-3">
      <User
        name="Лашпед"
        avatarProps={{
          fallback: (<p className="text-2xl select-none">❌</p>)
        }}
        description="2W/2L/2D (50%)"
      />
      <p className="text-lg font-semibold">1000⭐</p>
    </div>

    <div className="flex items-center gap-3">
      <p className="text-lg font-semibold">⭐1200</p>
      <User
        name="Лашпед"
        avatarProps={{
          fallback: (<p className="text-2xl select-none">⭕</p>)
        }}
        description="2W/2L/2D (50%)"
      />
    </div>
    {/* <GameJoinButton gameInfo={gameInfo}/> */}
  </div>
)

const RatingCard: React.FC = () => (
  <div className="flex justify-between items-center bg-default-100 rounded-lg p-2">
    <User
      name="Лашпед"
      description="2W/2L/2D (50%)"
    />
    <p className="text-lg font-semibold">1000⭐</p>
  </div>
)

const GameListPage: React.FC<{ page: GetGameListResponse }> = ({ page }) => {
  return page.games.map((x, i) => {
    if (x.state === GameStateEnum.NotStarted) {
      return (<GameCard key={i} gameInfo={x} />)
    }
    return (<GameStartedCard />)
  })
}

const GameList: React.FC = () => {
  const { data, hasNextPage, isFetchingNextPage } = useGetGameList(2);

  return (
    <>
      {data?.pages.map((x, i) => (<GameListPage key={i} page={x} />))}
      {/* <GameCard />
      <GameCard /> */}
      <GameStartedCard />
      {hasNextPage && <Button isLoading={isFetchingNextPage}>Еще</Button>}
    </>
  )
}

const RatingDivider: React.FC = () => <p className="text-[3.75rem] text-center line leading-3 translate-y-[-20px] text-foreground-300">...</p>;

const StartGameButton: React.FC = () => {
  const [maxRating, setMaxRating] = useState(1000);
  const navigate = useNavigate();
  const { mutateAsync, isPending } = useCreateGame({});

  async function create() {
    const result = await mutateAsync({ maxRating: maxRating });
    navigate(`/game/${result.gameId}`)
  }

  return (
    <Popover
      showArrow
      offset={10}
      placement="bottom"
      backdrop="blur"
    >
      <PopoverTrigger>
        <Button color="primary">Начать новую</Button>
      </PopoverTrigger>
      <PopoverContent>
        {(titleProps) => (
          <div className="px-1 py-2 w-full">
            <p className="text-small font-bold text-foreground" {...titleProps}>
              Создаание игры
            </p>
            <div className="mt-2 flex flex-col gap-2 w-full">
              <Input value={maxRating.toString()} onChange={x => setMaxRating(x.target.valueAsNumber)} label="Максимальный рейтинг" size="sm" variant="flat" type="number" />
              <Button
                onClick={create}
                isLoading={isPending}
                color="success">
                  Создать
              </Button>
            </div>
          </div>
        )}

      </PopoverContent>
    </Popover>
  )
}

export const RatingPage: React.FC = () => {


  return (
    <div className="rating-layout w-full h-full pt-16 pb-16">
      <Card className="w-[600px] col-[2] max-h-full">
        <CardHeader className="h-16 flex justify-between items-center px-4">
          <h2 className="text-2xl font-semibold">Игры</h2>
          <StartGameButton />
        </CardHeader>
        <Divider />
        <CardBody className="flex gap-2">
          <GameList />
        </CardBody>
      </Card>
      <Card className="w-[400px] col-[3] max-h-full">
        <CardHeader className="h-16 flex justify-between items-center px-4">
          <h2 className="text-2xl font-semibold">Рейтинг</h2>
        </CardHeader>
        <Divider />
        <CardBody className="flex gap-2">
          <RatingCard />
          <RatingCard />
          <RatingCard />
          <RatingCard />
          <RatingDivider />
          <RatingCard />
        </CardBody>
      </Card>
    </div>
  )
}
