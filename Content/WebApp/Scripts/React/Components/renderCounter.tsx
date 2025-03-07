import { useRef } from "react";

export const RenderCounter = (props) => {
  const renderCounter = useRef(0);
  renderCounter.current = renderCounter.current + 1;
  return (
    <span>
      {" "}
      Renders: {renderCounter.current} {props.message}
    </span>
  );
};
