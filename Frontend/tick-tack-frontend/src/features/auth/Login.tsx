import React from "react";
import img from "../../assets/tic-tac-toe.png";
import { Button, Card, CardBody, Input, Tab, Tabs } from "@nextui-org/react"
import { SubmitHandler, useForm } from "react-hook-form";
import { useAuth } from "@/lib/AuthProvider";
import { toast } from "react-toastify";
import { HTTPError } from "ky";


interface IRegisterFormInput {
  username: string;
  password: string;
  passwordConfirmation: string;
}

export const RegisterForm = () => {
  const { register, handleSubmit, formState: { errors }, setError } = useForm<IRegisterFormInput>()
  const { isRegistering, register: doRegister } = useAuth();

  const onSubmit: SubmitHandler<IRegisterFormInput> = async (data) => {
    try {
      await doRegister({ login: data.username, password: data.password })
    } catch (e) {
      if (e instanceof HTTPError && e.response.status == 400) {
        const formErrors = await e.response.json();
        if (formErrors.errors?.Login) {
          setError("username", { message: formErrors.errors?.Login.map((x) => x.message).join('\n') })
        }
      }
      toast("Ошибка", { type: "error" })
    }
  }

  return (
    <form onSubmit={handleSubmit(onSubmit)} className="h-full flex flex-col gap-4">
      <Input
        {...register("username", { required: "Обязательное поле" })}
        errorMessage={errors.username?.message}
        isInvalid={errors.username && true}
        isRequired
        label="Имя"
        placeholder="Имя"
        type="text"
        autoComplete="username"
      />
      <Input
        {...register("password")}
        isRequired
        label="Пароль"
        placeholder="Пароль"
        autoComplete="new-password"
        isInvalid={errors.password && true}
        type="password"
      />
      <Input
        {...register("passwordConfirmation", { validate: (x, y) => x === y.password ? undefined : "Пароли отличаются" })}
        isInvalid={errors.passwordConfirmation && true}
        errorMessage={errors.passwordConfirmation?.message}
        isRequired
        label="Повтор пароля"
        placeholder="Повтор пароля"
        autoComplete="new-password"
        type="password"
      />
      <div className="flex gap-2 mt-auto">
        <Button isLoading={isRegistering} type="submit" fullWidth color="primary" className="font-semibold text-medium">
          Зарегистрироваться
        </Button>
      </div>
    </form>
  )
}

interface ILoginFormInput {
  username: string;
  password: string;
}

export const LoginForm = () => {
  const { register, handleSubmit, formState: { errors }, setError } = useForm<ILoginFormInput>()
  const { isLoggingIn, login } = useAuth();

  const onSubmit: SubmitHandler<ILoginFormInput> = async (data) => {
    try {
      await login({ login: data.username, password: data.password })
    } catch (e) {
      if (e instanceof HTTPError && e.response.status == 400) {
        const formErrors = await e.response.json();
        if (formErrors.errors?.Password) {
          setError("password", { message: formErrors.errors?.Password.map((x) => x.message).join('\n') })
        }
      }
      if (e instanceof HTTPError && e.response.status == 404) {
        setError("username", { message: "Пользователь не найден" })
      }
      toast("Ошибка", { type: "error" })
    }
  }


  return (
    <form onSubmit={handleSubmit(onSubmit)} className="h-full flex flex-col gap-4">
      <Input
        {...register("username")}
        isInvalid={errors.username && true}
        errorMessage={errors.username?.message}
        isRequired
        label="Имя"
        placeholder="Имя"
        type="text"
      />
      <Input
        {...register("password")}
        isInvalid={errors.password && true}
        errorMessage={errors.password?.message}
        isRequired
        label="Пароль"
        placeholder="Пароль"
        type="password"
      />
      <div className="flex gap-2 mt-auto">
        <Button isLoading={isLoggingIn} type="submit" fullWidth color="primary" className="font-semibold text-medium">
          Войти
        </Button>
      </div>
    </form>
  )
}

export const LoginPage: React.FC = () => {
  const [selected, setSelected] = React.useState("login");
  const { isLoggingIn, isRegistering } = useAuth();

  return (
    <div className="flex flex-col w-full h-full justify-center items-center">
      <Card className="max-w-full w-[340px] h-[480px]">
        <CardBody className="overflow-hidden">
          <div className="flex justify-center items-center my-4 gap-2">
            <img className="w-12" src={img} />
            <p className="font-bold text-4xl">XoX</p>
          </div>
          <Tabs
            isDisabled={isLoggingIn || isRegistering}
            fullWidth
            size="md"
            aria-label="Tabs form"
            selectedKey={selected}
            onSelectionChange={(x) => setSelected(x.toString())}
            classNames={{
              panel: "h-full",
              base: "mb-2"
            }}
          >
            <Tab key="login" title="Вход" className="font-semibold">
              <LoginForm />
            </Tab>
            <Tab key="sign-up" title="Регистрация" className="font-semibold">
              <RegisterForm />
            </Tab>
          </Tabs>
        </CardBody>
      </Card>
    </div>
  );
}