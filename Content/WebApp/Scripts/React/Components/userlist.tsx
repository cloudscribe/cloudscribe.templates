import { useQuery } from "@tanstack/react-query";
import Axios from "axios";
import { RenderCounter } from "./renderCounter";

export interface UserModel {
    userName: string;
    email: string;
    createdUtc: string;
}

export const UserList = () => {
    const {
        data: userData,
        isLoading,
        isFetching,
        isError,
        refetch,
        error,
    } = useQuery<UserModel[]>({
        queryKey: ["userData"],
        queryFn: () => {
            return Axios.get("/csapi/getusers").then((res) => res.data);
        },
    });

    if (isLoading) {
        return <h1>Loading...</h1>;
    }

    if (isFetching) {
        return <h1>Re-fetching...</h1>;
    }

    if (isError) {
        return <h1>Error: {error.message}</h1>;
    }

    return (
        <div>
            <h2>React queried this user data from the cloudscribe API:</h2>
            {userData.map((user) => (
                <div key={user.userName}>
                    {user.userName} - {user.email} - created {user.createdUtc}
                </div>
            ))}
            <hr />
            <RenderCounter message="component renders" />

            <br />
            <button onClick={() => refetch()}>Re-fetch</button>
        </div>
    );
};
