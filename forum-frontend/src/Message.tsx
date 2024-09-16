interface Props {
  message: string;
}

function Message(props: Props) {
  return <h1 className="fw-bold">{props.message}</h1>;
}

export default Message;
