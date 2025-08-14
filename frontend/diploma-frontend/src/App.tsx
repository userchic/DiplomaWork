import Layout, { Content, Footer, Header } from "antd/es/layout/layout";
import { Menu } from "antd";
import { Link, Route, Routes } from "react-router";

import { Logout } from "./Services/HomeService";

import useAuth from "./Hooks/useAuth";
import './App.css'
import AuthContext from "./Contexts/AuthContext";
import Games from "./Components/Games";
import LoginSection from "./Components/Login";
import { PrivateRoute } from "./Components/PrivateRoute";
import Registry from "./Components/Registry";
import { useState } from "react";
import Tasks from "./Components/Tasks";
import Task from "./Components/Task";
import Students from "./Components/Students";
import Student from "./Components/Student";
import TaskCreator from "./Components/TaskCreator";
import StudentCreator from "./Components/StudentCreator";
import TaskGenerator from "./Components/TaskGenerator";
import GameCreator from "./Components/GameCreator";
import GameStartConfirmation from "./Components/GameComponents/GameStartConfirmation";
import PreStartActions from "./Components/GameComponents/PreStartActions";
import Timer from "./Components/GameComponents/Timer";
import CaptainsCompetetion from "./Components/GameComponents/CaptainsCompetetion";
import GameInterface from "./Components/GameComponents/GameInterface";


function App() {
  const [isAuthenticated, setAuth] = useState<boolean>(false);
  function ExecuteLogout(): void {
    Logout();
    setAuth(false)

  }

  return (


    <Layout style={{ minHeight: "100vh" }}>
      <Header>
        <Menu theme="dark"
          mode="horizontal"
          items={isAuthenticated ? [
            { key: "games", label: <Link to={"/Games"}>Бои</Link> },
            { key: "tasks", label: <Link to={"/Tasks"}>Задачи</Link> },
            { key: "students", label: <Link to={"/Students"}>Студенты</Link> },
            { key: "logout", label: <input type="button" onClick={ExecuteLogout} value="Выход" /> },
          ] : [
            { key: "login", label: <Link to={"/Login"}>Вход</Link> },
            { key: "Registry", label: <Link to={"/Registry"}>Регистрация</Link> },

          ]}
          style={{ flex: 1, minWidth: 0 }}
        ></Menu>
      </Header>
      <Content style={{ padding: "0 48px" }}>
        <AuthContext.Provider value={{ isAuthenticated, setAuth }}>
          <Routes>
            <Route path="/Login" element={<LoginSection />} />
            <Route path="/Registry" element={<Registry />} />
            <Route element={<PrivateRoute />}>
              <Route path="/Games" element={<Games />} />
              <Route path="/GameStartConfirmation/:gameId" element={<GameStartConfirmation />} />
              <Route path="/PreStartActions/:gameId" element={<PreStartActions />} />
              <Route path="/CaptainsCompetetion/:gameId" element={<CaptainsCompetetion />} />
              <Route path="/Timer/:gameId" element={<Timer />} />
              <Route path="/GameInterface/:gameId" element={<GameInterface />} />
              <Route path="/GameCreate" element={<GameCreator />} />
              <Route path="/Task/:id" element={<Task />} />
              <Route path="/Tasks" element={<Tasks />} />
              <Route path="/TaskCreate" element={<TaskCreator />} />
              <Route path="/TaskGenerator" element={<TaskGenerator />} />
              <Route path="/Student/:id" element={<Student />} />
              <Route path="/Students" element={<Students />} />
              <Route path="/StudentCreate" element={<StudentCreator />} />
            </Route>
          </Routes>
        </AuthContext.Provider>
      </Content>
      <Footer style={{ textAlign: "center" }}>
        Math Battles 2025 Created by me
      </Footer>
    </Layout>

  );
}


export default App
