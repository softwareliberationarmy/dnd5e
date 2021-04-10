import React, { useReducer } from "react";
import RollResult from "../../components/Roll/RollResult";
import ErrorMessage from "../../components/Error/ErrorMessage";
import RollRequestor from "../../components/Roll/RollRequestor";
import RollService from "../../services/RollService";

const FreeRoller = () => {
  const [state, dispatch] = useReducer(reducer, initialState);

  return (
    <div className="Page-header">
      <h1>Roll your fate</h1>
      <RollResult roll={state.roll} />
      <ErrorMessage error={state.error} />
      <RollRequestor requested={(rollType) => makeRoll(rollType, dispatch)} />
    </div>
  );
};

const initialState = { loading: false, roll: null, error: null };
export const reducer = (state, action) => {
  switch (action.type) {
    case "loading":
      return { loading: true, roll: null, error: null };
    case "success":
      return { loading: false, roll: action.payload, error: null };
    case "error":
      return { loading: false, roll: null, error: action.payload };
    default:
      return {};
  }
};

export const makeRoll = (rollType, dispatch) => {
  if (rollType) {
    dispatch({ type: "loading" });
    return RollService.rollDice(rollType)
      .then((result) => {
        dispatch({ type: "success", payload: result.data.result });
      })
      .catch((err) => {
        if (err.response && err.response.status === 400) {
          //bad request
          dispatch({
            type: "error",
            payload: "You entered an invalid roll request.",
          });
        } else {
          dispatch({
            type: "error",
            payload: "Error rolling dice. Please try again.",
          });
        }
      });
  }
};

export default FreeRoller;
